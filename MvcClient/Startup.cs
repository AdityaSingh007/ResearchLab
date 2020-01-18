using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using MvcClient.App_Start;
using MvcClient.Utils;
using Owin;

[assembly: OwinStartup(typeof(MvcClient.Startup))]

namespace MvcClient
{
    public class Startup
    {


        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
    new OpenIdConnectAuthenticationOptions
    {
        ClientId = AuthenticationConfig.clientId,
        Authority = AuthenticationConfig.authority,
        RedirectUri = AuthenticationConfig.redirectUri,
        PostLogoutRedirectUri = AuthenticationConfig.postLogoutRedirectUri,
        Resource = AuthenticationConfig.serviceResourceID,
        //Scope = AuthenticationConfig.microsoftGraphScope,
        TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            RoleClaimType = "roles"
        },
        Notifications = new OpenIdConnectAuthenticationNotifications
        {
            AuthorizationCodeReceived = OnAuthorizationCodeReceived,
            AuthenticationFailed = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/Home/Error");
                return Task.FromResult(0);
            }
        }
    });
        }

        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification notification)
        {
            // Extract the code from the response notification
            var code = notification.Code;
            string signedInUserID = notification.AuthenticationTicket.Identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            var confidentialClientApplication = MSALAppBuilder.GetAppBuilder(signedInUserID,
                notification.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase);

            try
            {
                AuthenticationResult result = await confidentialClientApplication.AcquireTokenByAuthorizationCodeAsync(code, new List<string>() { AuthenticationConfig.scopes });
            }
            catch (MsalException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                //TODO: Handle
                throw;
            }
        }
    }


}
