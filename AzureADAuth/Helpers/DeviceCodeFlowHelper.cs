using AzureADAuth.Models;
using IdentityModel;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureADAuth.Helpers
{
    public class DeviceCodeFlowHelper
    {
        static IDiscoveryCache _cache = new DiscoveryCache("https://demo.identityserver.io");

        public static DeviceAuthorizationResponse DeviceAuthorizationResponse { get; set; }
        public static async Task<DeviceAuthorizationResponse> RequestAuthorizationAsync()
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();
            var response = await client.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
            {
                Address = disco.DeviceAuthorizationEndpoint,
                ClientId = "device"
            });

            if (response.IsError) throw new Exception(response.Error);
            DeviceAuthorizationResponse = response;
            return response;
        }

        public static async Task<TokenResponse> RequestTokenAsync(DeviceAuthorizationResponse authorizeResponse)
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();

            while (true)
            {
                var response = await client.RequestDeviceTokenAsync(new DeviceTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "device",
                    DeviceCode = authorizeResponse.DeviceCode
                });


                if (response.IsError)
                {
                    if (response.Error == OidcConstants.TokenErrors.AuthorizationPending || response.Error == OidcConstants.TokenErrors.SlowDown)
                    {
                        await Task.Delay(authorizeResponse.Interval * 1000);
                    }
                    else
                    {
                        throw new Exception(response.Error);
                    }
                }
                else
                {
                    return response;
                }
            }
        }

    }
}
