using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AzureADAuth.Services
{
    public class ClientCredentialTokenService
    {
        private HttpClient _tokenServiceClient = new HttpClient();
        private TokenResponse _tokenResponse = new TokenResponse();
        private string _token = string.Empty;
        private bool _isTokenExpired = false;
        private readonly HttpContext _httpContext;
        private readonly ClientCredentialsTokenRequest _tokenRequestParameters;
        public ClientCredentialTokenService(HttpContext httpContext, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        {
            _httpContext = httpContext;
            _tokenRequestParameters = clientCredentialsTokenRequest;
        }

        private bool IsTokenExpired()
        {
            if (!string.IsNullOrEmpty(_httpContext.Session.GetInt32("expires_in").ToString()))
            {
                var expires_At = DateTime.UtcNow + TimeSpan.FromSeconds(Convert.ToDouble(_httpContext.Session.GetInt32("expires_in")));
                _isTokenExpired = ((expires_At.AddSeconds(-60)).ToUniversalTime() < DateTime.UtcNow);
            }
            return _isTokenExpired;
        }

        private string GetTokenEndPoint()
        {
            var azureADDiscoveryDocumentResponse = _tokenServiceClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = "https://login.microsoftonline.com/188697ab-840f-44ec-8535-aaaa5680bab0/v2.0/.well-known/openid-configuration",
                Policy =
                {
                    ValidateIssuerName=false,
                    ValidateEndpoints=false
                }
            }).Result;
            _tokenServiceClient.Dispose();

            var azureADTokenEndpoint = azureADDiscoveryDocumentResponse.TokenEndpoint;
            return azureADTokenEndpoint;
        }

        private string GetTokenFromInMemory()
        {
            var storedToken = string.Empty;
            if (!String.IsNullOrEmpty(_httpContext.Session.GetString("access_token")) && (IsTokenExpired() == false))
            {
                storedToken = _httpContext.Session.GetString("access_token");
            }
            return storedToken;
        }

        private string GetTokenFromTokenEndPoint()
        {
            var tokenEndPoint = GetTokenEndPoint();
            _tokenServiceClient = new HttpClient();
            _tokenServiceClient.BaseAddress = new Uri(tokenEndPoint);
            _tokenServiceClient.DefaultRequestHeaders.Accept.Clear();
            _tokenServiceClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _tokenResponse = _tokenServiceClient.RequestClientCredentialsTokenAsync(_tokenRequestParameters)
                                                .Result;

            if (!string.IsNullOrEmpty(_tokenResponse.AccessToken))
            {
                _httpContext.Session.SetString("access_token", _tokenResponse.AccessToken);
                _httpContext.Session.SetInt32("expires_in", _tokenResponse.ExpiresIn);
            }

            _tokenServiceClient.Dispose();

            return _tokenResponse.AccessToken;
        }

        public string GetToken()
        {
            if (!string.IsNullOrEmpty(GetTokenFromInMemory()))
                _token = GetTokenFromInMemory();
            else
                _token = GetTokenFromTokenEndPoint();
            return _token;
        }
    }
}
