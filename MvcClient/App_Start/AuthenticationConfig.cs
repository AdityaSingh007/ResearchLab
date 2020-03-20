using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MvcClient.App_Start
{
    public static class AuthenticationConfig
    {
        public static readonly string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static readonly string appKey = ConfigurationManager.AppSettings["ida:appKey"];
        public static readonly string aadInstance = ConfigurationManager.AppSettings["ida:aadInstance"];
        public static readonly string tenant = ConfigurationManager.AppSettings["ida:tenant"];
        public static readonly string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:postLogoutRedirectUri"];
        public static readonly string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        public static readonly string serviceResourceID = ConfigurationManager.AppSettings["ida:serviceResourceID"];
        public static readonly string azureManagementResourceID = "https://management.azure.com";

        public static readonly string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        public static readonly string scopes = "openid profile offline_access";
        public static readonly string microsoftGraphScope = "https://graph.microsoft.com";
        public static readonly string storageAccountScope = "https://storage.azure.com/user_impersonation";
        public static readonly string azureManagementScope = "https://management.azure.com/user_impersonation";
    }
}