using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureADAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using IdentityModel.Client;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using AzureADAuth.Services;

namespace AzureADAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient = new HttpClient();

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("http://localhost:1601/");
        }

        [Route("")]
        //[Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Route("userinformation")]
        [Authorize]
        public async Task<IActionResult> UserInformation()
        {
            string accessToken = string.Empty;
            string refreshToken = string.Empty;
            string idToken = string.Empty;
            string authCode = string.Empty;

            // get the current HttpContext to access the tokens
            var currentContext = _httpContextAccessor.HttpContext;

            accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            refreshToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            idToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            authCode = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.Code);
            var expires_at = await currentContext.GetTokenAsync("expires_at");

            var ownerName = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            var ownerRole = User.Claims.FirstOrDefault(c => c.Type == "roles").Value;

            var isInAdminRole = User.IsInRole("Admin");

            var expirationTime = ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime());

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (string.IsNullOrWhiteSpace(expires_at)
               || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime() < DateTime.UtcNow))
            {
                accessToken = await RenewTokens();
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // set as Bearer token
                _httpClient.SetBearerToken(accessToken);
            }

            var response = _httpClient.GetAsync("api/Azure").Result.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string>(response.Result);


            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> RenewTokens()
        {
            // get the current HttpContext to access the tokens
            var currentContext = _httpContextAccessor.HttpContext;

            var azureADDiscoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = "https://login.microsoftonline.com/71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa",
                Policy =
                {
                    ValidateIssuerName=false,
                    ValidateEndpoints=false
                }
            });

            var azureADTokenEndpoint = azureADDiscoveryDocumentResponse.TokenEndpoint;

            // get the saved refresh token
            var currentRefreshToken = await currentContext
                .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var renewedTokens = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                ClientId = "c3f6e601-6398-45cf-b392-39d7c28bc110",
                ClientSecret = "d8z@UAOHE=Vmx-WuTmN3d24tJt/Y?Fh[",
                Address = azureADTokenEndpoint,
                RefreshToken = currentRefreshToken
            });

            if (!renewedTokens.IsError)
            {
                // update the tokens & exipration value
                var updatedTokens = new List<AuthenticationToken>();
                updatedTokens.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = renewedTokens.IdentityToken
                });
                updatedTokens.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = renewedTokens.AccessToken
                });
                updatedTokens.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = renewedTokens.RefreshToken
                });

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(renewedTokens.ExpiresIn);
                updatedTokens.Add(new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                });

                // get authenticate result, containing the current principal & 
                // properties
                var currentAuthenticateResult = await currentContext.AuthenticateAsync("Cookies");

                // store the updated tokens
                currentAuthenticateResult.Properties.StoreTokens(updatedTokens);

                // sign in
                await currentContext.SignInAsync("Cookies",
                    currentAuthenticateResult.Principal,
                    currentAuthenticateResult.Properties);
            }

            var renewedAccesToken = renewedTokens.AccessToken;
            var renewedIdToken = renewedTokens.IdentityToken;
            var renewedrefreshToken = renewedTokens.RefreshToken;

            return renewedAccesToken;
        }

        [Route("ServerToServerCommunication")]
        public async Task<IActionResult> ServerToServerCommunication()
        {
            try
            {
                var apiResponse = string.Empty;
                var tokenRequestParameters = new ClientCredentialsTokenRequest()
                {
                    GrantType = "client_credentials",
                    ClientId = "94352d85-f554-4754-90a9-2eec3d75e68c",
                    ClientSecret = "]2Rhx?IIg_J0-EGJWacUHuOgoMeLKD12",
                    Scope = "https://storage.azure.com/.default"
                };

                var tokenService = new ClientCredentialTokenService(HttpContext, tokenRequestParameters);
                var access_token = tokenService.GetToken();

                if (!string.IsNullOrEmpty(access_token))
                {
                    _httpClient.SetBearerToken(access_token);
                    var response = await _httpClient.GetAsync("https://conpeusbmpsgdata01sav2.dfs.core.windows.net/?resource=account")
                                              .Result
                                              .Content
                                              .ReadAsStringAsync();

                    //apiResponse = JsonConvert.DeserializeObject<string>(response.Result);
                    apiResponse = response;
                }

                return Json(new { data = new { access_token = access_token, apiResponse = apiResponse }, status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {

                return Json(new { data = new { errorDescription = ex.InnerException != null ? ex.InnerException.Message : ex.Message },
                    status = HttpStatusCode.InternalServerError });
            }


        }
    }
}
