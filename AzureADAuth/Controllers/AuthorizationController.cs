using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace AzureADAuth.Controllers
{
    public class AuthorizationController : Controller
    {
        private static string Message { get; set; } = "";
        private static string Code { get; set; }
        private static string Token { get; set; }
        private static string TokenType { get; set; }

        private static readonly string State = CryptoRandom.CreateUniqueId();

        private const string ClientId = "c3f6e601-6398-45cf-b392-39d7c28bc110";
        private const string ClientSecret = "d8z@UAOHE=Vmx-WuTmN3d24tJt/Y?Fh[";
        private const string RedirectUri = "http://localhost:54295/callback";

        public IActionResult Index()
        {
            return View("Index", Message);
        }

        public IActionResult Authorize()
        {
            var authUrl =
                $"https://login.microsoftonline.com/71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa/oauth2/v2.0/authorize?client_id={ClientId}&scope=2716d103-ccb8-4499-9471-df3becb40f94/.default offline_access&redirect_uri={RedirectUri}&response_type=code&response_mode=query&state={State}";

            Message += $"Redirecting to authorization endpoint. State value of: {State}";
            Message += $"\n<b>URL:</b> {authUrl}";
            return Redirect(authUrl);
        }

        [Route("Callback")]
        public IActionResult Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (State != state)
            {
                Message += "\n\nState not recognised. Cannot trust response.";
                return RedirectToAction("Index");
            }

            Code = code;

            Message += "\n\nApplication Authorized!";
            Message += $"\n<b>code:</b> {code}";
            Message += $"\n<b>state:</b> {State}";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetToken()
        {
            var httpClient = new HttpClient();

            if (Code == null)
            {
                Message += "\n\nNot ready! Authorize first.";
                return RedirectToAction("Index");
            }

            Message += "\n\nCalling token endpoint...";

            var tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Address = "https://login.microsoftonline.com/71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa/oauth2/v2.0/token",
                Code = Code,
                RedirectUri = RedirectUri
            });


            if (tokenResponse.IsError)
            {
                Message += "\n\nToken request failed";
                return RedirectToAction("Index");
            }

            TokenType = tokenResponse.TokenType;
            Token = tokenResponse.AccessToken;
            Message += "\n\nToken Received!";
            Message += $"\n<b>access_token:</b> {tokenResponse.AccessToken}";
            Message += $"\n<b>refresh_token:</b> {tokenResponse.RefreshToken}";
            Message += $"\n<b>expires_in:</b> {tokenResponse.ExpiresIn}";
            Message += $"\n<b>token_type:</b> {tokenResponse.TokenType}";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CallApi()
        {
            var httpClient = new HttpClient();
            if (Token != null)
            {
                Message += $"\n\nCalling API with Authorization header: {TokenType} {Token}";
                httpClient.SetBearerToken(Token);
            }

            var response = await httpClient.GetAsync("http://localhost:1601/api/azure");

            if (response.IsSuccessStatusCode) Message += "\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }
    }
}