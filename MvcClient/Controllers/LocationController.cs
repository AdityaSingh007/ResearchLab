using Microsoft.Identity.Client;
using MvcClient.App_Start;
using MvcClient.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private static string serviceBaseAddress = "http://localhost:60723/"; // base url of the web api
        public async Task<ActionResult> Index()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            var confidentialClientApplication = MSALAppBuilder.GetAppBuilder(signedInUserID, this.HttpContext);
            var userRole = User.IsInRole("Admin");
            var user = await confidentialClientApplication.GetAccountsAsync();

            AuthenticationResult result = await confidentialClientApplication.AcquireTokenSilentAsync(new List<string>() { AuthenticationConfig.scopes },
                user.FirstOrDefault(), AuthenticationConfig.authority, false);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceBaseAddress + "values/GetLocation");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.coordinates = apiResponse;
                return View("Index");
            }
            else
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                }
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View("Index");
            }
        }
    }
}