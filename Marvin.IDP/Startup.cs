using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Marvin.IDP.Entities;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marvin.IDP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["connectionStrings:marvinUserDBConnectionString"];
            services.AddDbContext<MarvinUserContext>(o => o.UseSqlServer(connectionString));

            var identityServerDataDBConnectionString =
               Configuration["connectionStrings:identityServerDataDBConnectionString"];

            var migrationsAssembly = typeof(Startup)
                .GetTypeInfo().Assembly.GetName().Name;


            services.AddScoped<IMarvinUserRepository, MarvinUserRepository>();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddMvc();
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddMarvinUserStore()
                   .AddConfigurationStore(options =>
                   {
                       options.ConfigureDbContext = builder =>
                           builder.UseSqlServer(identityServerDataDBConnectionString,
                               sql => sql.MigrationsAssembly(migrationsAssembly));
                   })
                   .AddOperationalStore(options =>
                   {
                       options.ConfigureDbContext = builder =>
                       builder.UseSqlServer(identityServerDataDBConnectionString,
                              sql => sql.MigrationsAssembly(migrationsAssembly));
                   }
                   );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            MarvinUserContext marvinUserContext,
            ConfigurationDbContext configurationDbContext,
            PersistedGrantDbContext persistedGrantDbContext)
        {

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            configurationDbContext.Database.Migrate();
            configurationDbContext.EnsureSeedDataForContext();

            marvinUserContext.Database.Migrate();
            marvinUserContext.EnsureSeedDataForContext();

            persistedGrantDbContext.Database.Migrate();

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
