using Microsoft.Identity.Client;
using MvcClient.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcClient.Utils
{
    public static class MSALAppBuilder
    {
        public static ConfidentialClientApplication GetAppBuilder(string signedInUserId, HttpContextBase httpContextBase)
        {
            TokenCache userTokenCache = new MSALSessionCache(signedInUserId, httpContextBase).GetMsalCacheInstance();
            ClientCredential clientCredential = new ClientCredential(AuthenticationConfig.appKey);
            ConfidentialClientApplication confidentialClientApplication = new ConfidentialClientApplication(AuthenticationConfig.clientId,
                                                                                                            AuthenticationConfig.authority,
                                                                                                            AuthenticationConfig.redirectUri,
                                                                                                            clientCredential,
                                                                                                            userTokenCache,
                                                                                                            null);

            return confidentialClientApplication;
        }
    }
}