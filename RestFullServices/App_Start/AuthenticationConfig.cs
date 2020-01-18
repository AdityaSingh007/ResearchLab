using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RestFullServices.App_Start
{
    public static class AuthenticationConfig
    {
        public static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string appKey = ConfigurationManager.AppSettings["ida:appKey"];
        public static string aadInstance = ConfigurationManager.AppSettings["ida:aadInstance"];
        public static string tenant = ConfigurationManager.AppSettings["ida:tenant"];
        public static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:postLogoutRedirectUri"];
        public static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        public static string serviceResourceID = ConfigurationManager.AppSettings["ida:serviceResourceID"];

        public static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
    }
}