using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Clients;

[assembly: OwinStartupAttribute(typeof(Web.Startup))]
namespace Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            //ResourceOwner password flow
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString("/Home/Login")
            });

            //ImplicitFlow 
            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
            //{
            //    ClientId = "socialnetwork_implicit",
            //    Authority = "http://localhost:58457/",
            //    RedirectUri = "http://localhost:41109",
            //    ResponseType = "token id_token",
            //    Scope = "openid profile Read",
            //    SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    RequireHttpsMetadata = false,
            //    PostLogoutRedirectUri = "http://localhost:41109",
            //    Notifications = new OpenIdConnectAuthenticationNotifications()
            //    {
            //        SecurityTokenValidated = notification =>
            //          {
            //              var identity = notification.AuthenticationTicket.Identity;
            //              identity.AddClaim(new System.Security.Claims.Claim("id_token", notification.ProtocolMessage.IdToken));
            //              identity.AddClaim(new System.Security.Claims.Claim("access_token", notification.ProtocolMessage.AccessToken));
            //              notification.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
            //              return Task.FromResult(0);
            //          },
            //        RedirectToIdentityProvider = notification =>
            //        {
            //            if (notification.ProtocolMessage.RequestType != Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectRequestType.Logout)
            //                return Task.FromResult(0);

            //            notification.ProtocolMessage.IdTokenHint = notification.OwinContext.Authentication.User.FindFirst("id_token").Value;
            //            return Task.FromResult(0);
            //        }
            //    }

            //});

            //Authorization code flow 
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
            {
                ClientId = "scoialnetwork_code",
                Authority = "http://localhost:58457/",
                RedirectUri = "http://localhost:41109/",
                ResponseType = "code id_token",
                Scope = "openid profile Read offline_access Role",
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                RequireHttpsMetadata = false,
                PostLogoutRedirectUri = "http://localhost:41109",
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthorizationCodeReceived = async notification =>
                    {

                        var requestResponse = await OidcClient.CallTokenEndpointAsync(
                            new Uri("http://localhost:58457/connect/token"),
                            new Uri("http://localhost:41109/"),
                            notification.Code,
                            "scoialnetwork_code",
                            "secret"
                            );

                        var identity = notification.AuthenticationTicket.Identity;

                        identity.AddClaim(new System.Security.Claims.Claim("id_token", requestResponse.IdentityToken));
                        identity.AddClaim(new System.Security.Claims.Claim("access_token", requestResponse.AccessToken));
                        identity.AddClaim(new System.Security.Claims.Claim("refresh_token", requestResponse.RefreshToken ?? string.Empty));

                        notification.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(identity,
                            notification.AuthenticationTicket.Properties);

                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType != Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectRequestType.Logout)
                            return Task.FromResult(0);

                        notification.ProtocolMessage.IdTokenHint = notification.OwinContext.Authentication.User.FindFirst("id_token").Value;
                        return Task.FromResult(0);
                    }
                },
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //NameClaimType = "name",
                    RoleClaimType = "Role"
                }
            });
        }
    }
}
