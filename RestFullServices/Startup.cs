using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using RestFullServices.Autofac.Modules;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using RestFullServices.App_Start;
using Microsoft.Owin.Security.Cookies;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;

[assembly: OwinStartup(typeof(RestFullServices.Startup))]

namespace RestFullServices
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var config = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<SocialNetworkModule>();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions()
            //{
            //    Authority = "http://localhost:58457"
            //});

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var azureADBearerAuthOptions = new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Tenant = AuthenticationConfig.tenant
            };

            azureADBearerAuthOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidAudience = AuthenticationConfig.clientId,
                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role
            };

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(azureADBearerAuthOptions);

        }
    }
}
