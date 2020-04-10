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
using Microsoft.AspNetCore.Authentication.Google;
using System.Text;
using AzureADAuth.Helpers;

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

            var response = _httpClient.GetAsync("api/Azure").Result.ReasonPhrase;
            //var result = JsonConvert.DeserializeObject<string>(response.Result.ReasonPhrase);


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

                return Json(new
                {
                    data = new { errorDescription = ex.InnerException != null ? ex.InnerException.Message : ex.Message },
                    status = HttpStatusCode.InternalServerError
                });
            }


        }

        [Route("AssignRole")]
        [Authorize]
        public async Task<IActionResult> AssignRole()
        {
            var roleAssignmentModel = new RoleAssisgnmentModel()
            {
                SubscriptionId = "47ca3602-b986-46de-a99a-e473c26bd588",
                ResourceGroupName = "AdityaAzureRG",
                StorageAccountName = "researchstorageacct",
                ContainerName = "research",
                RoleId = "2a2b9908-6ea1-4ae2-8e65-a410df84e7d1",
                ServicePrincipalObjectId = "04f300cc-5632-4820-8fd2-9d36e7efd020",
                RoleAssignmentName = Guid.NewGuid().ToString()
            };

            var roleAssignmentService = new RoleAssignment(roleAssignmentModel);

            var currentContext = _httpContextAccessor.HttpContext;
            var accesToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var httpResponseMessage = await roleAssignmentService.AssignRole(accesToken);
            var responeFromazureAPi = await httpResponseMessage.Content.ReadAsStringAsync();
            var deserializeResponse = JsonConvert.DeserializeObject(responeFromazureAPi).ToString();

            return View("AssignRole", deserializeResponse);
        }

        [Route("CookieAuthenticationDemo")]
        [Authorize]
        public IActionResult CookieAuthDemo()

        {
            var tokenIssuedByGoogle = _httpContextAccessor.HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
            return View();
        }

        [HttpGet]
        [Route("AuthorizationByPassingModelDemo")]
        public async Task<IActionResult> AuthorizationByPassingModelDemo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                /* Redirects the user to the authentication screen */
                return new ChallengeResult();
            }
            var requestBody = new { UserName = "Aditya", Age = 23 };

            var authResponse = _httpClient.PostAsync("api/Azure/AuthorizeUsingCustomModel",
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));

            var response = await authResponse.Result
                                              .Content
                                              .ReadAsStringAsync();

            return Ok(JsonConvert.DeserializeObject(response));
        }

        [HttpGet]
        public async Task<IActionResult> PKCE()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeviceCode()
        {
            try
            {
                var authResponse = await DeviceCodeFlowHelper.RequestAuthorizationAsync();
                //var tokenResponse = await DeviceCodeFlowHelper.RequestTokenAsync(authResponse);
                return View(new DeviceCodeAuthorizationResponse()
                {
                    UserCode = authResponse.UserCode,
                    DeviceCode = authResponse.DeviceCode,
                    VerificationUri = authResponse.VerificationUri,
                    VerificationUriComplete = authResponse.VerificationUriComplete
                });
            }
            catch (Exception ex)
            {
                return View(new DeviceCodeAuthorizationResponse()
                {
                    error = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessTokenForDevice()
        {
            try
            {
                var tokenResponse = await DeviceCodeFlowHelper.RequestTokenAsync(DeviceCodeFlowHelper.DeviceAuthorizationResponse);
                return Json(new { accessToken = tokenResponse.AccessToken,
                    DeviceCode = DeviceCodeFlowHelper.DeviceAuthorizationResponse.DeviceCode,
                    UserCode = DeviceCodeFlowHelper.DeviceAuthorizationResponse.UserCode });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }
        }


    }
}
