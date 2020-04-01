using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web.Models
{
    public static class MicrosoftAuthenticationManager
    {
        public static IConfidentialClientApplication confidentialClientApplication;

        public async static Task<string> AcquireToken()
        {
            try
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder.Create("1e4f9c1e-9a8b-4fc6-a128-5c91050bc96f")
                                                                              .WithClientSecret("/N=:6n1O:SW07=A-DppgbV3D_FP3Ul5m")
                                                                              .WithRedirectUri("http://localhost:41109/")
                                                                              .WithAuthority("https://login.microsoftonline.com", "71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa")
                                                                              .Build();

                var authResult = await confidentialClientApplication.AcquireTokenForClient(new List<string>() { "https://graph.microsoft.com/.default" })
                                                                    .ExecuteAsync();
                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}