using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;
using IdentityServer3.Core.Models;
using Microsoft.Owin;
using Owin;
using SocialNetwork.Data.Repositories;
using System.Data.SqlClient;
using IdentityServer3.Core.Services.Default;

[assembly: OwinStartup(typeof(OAuthServer.Startup))]

namespace OAuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var entityFrameworkOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SocialNetwork.IdSvr"].ConnectionString
            };
            var inmemoryManager = new InMemoryManager();

            var userRepository = new UserRepository(
                () => new SqlConnection(ConfigurationManager.ConnectionStrings["SocialNetwork"].ConnectionString)
                );

            var viewServiceOptions = new DefaultViewServiceOptions();
            //viewServiceOptions.Stylesheets.Add("/css/bootstrap.min.css");

            var factory = new IdentityServerServiceFactory();
            SetupClients(inmemoryManager.GetClients(), entityFrameworkOptions);
            SetupScopes(inmemoryManager.GetScopes(), entityFrameworkOptions);

            factory.RegisterConfigurationServices(entityFrameworkOptions);
            factory.RegisterOperationalServices(entityFrameworkOptions);
            factory.UserService = new Registration<IdentityServer3.Core.Services.IUserService>(typeof(SocialNetworkUserService));
            factory.Register(new Registration<IUserRepository>(userRepository));
            factory.ConfigureDefaultViewService<CustomViewService>(viewServiceOptions);

            new TokenCleanup(entityFrameworkOptions, 1).Start();

            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);

            var options = new IdentityServerOptions()
            {
                SiteName="Facenotebook!!!",
                SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate, "password"),
                RequireSsl = false,
                Factory = factory
            };

            app.UseIdentityServer(options);
        }

        public void SetupClients(IEnumerable<Client> clients, EntityFrameworkServiceOptions entityFrameworkServiceOptions)
        {
            using (var context = new ClientConfigurationDbContext(
                entityFrameworkServiceOptions.ConnectionString, entityFrameworkServiceOptions.Schema))
            {
                if (context.Clients.Any())
                {
                    return;
                }

                foreach (var client in clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }
        }

        public void SetupScopes(IEnumerable<Scope> scopes, EntityFrameworkServiceOptions entityFrameworkServiceOptions)
        {
            using (var context = new ScopeConfigurationDbContext(
                entityFrameworkServiceOptions.ConnectionString, entityFrameworkServiceOptions.Schema))
            {
                if (context.Scopes.Any())
                    return;

                foreach (var scope in scopes)
                {
                    context.Scopes.Add(scope.ToEntity());
                }

                context.SaveChanges();
            }

        }
    }
}
