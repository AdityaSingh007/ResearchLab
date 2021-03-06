﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureADAuth.Data;
using AzureADAuth.Entities;
using AzureADAuth.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AzureADAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ImageStore>();
            services.AddScoped<IUserRepository, UserRepository>();

            var connectionString = Configuration["ConnectionStrings:salesDBConnectionString"];
            services.AddDbContext<SalesContext>(o => o.UseSqlServer(connectionString));

            //Add below to enable use of session variable
            services.AddSession();

            ///***Uncomment below to enable Azure AD Authentication***///

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddOpenIdConnect(options =>
            //{
            //    options.Authority = "https://login.microsoftonline.com/71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa";
            //    options.ClientId = "c3f6e601-6398-45cf-b392-39d7c28bc110";
            //    options.ClientSecret = "d8z@UAOHE=Vmx-WuTmN3d24tJt/Y?Fh[";
            //    options.ResponseType = "code id_token";
            //    //options.Resource = "2716d103-ccb8-4499-9471-df3becb40f94";
            //    options.Resource = "https://management.azure.com";
            //    options.Scope.Add("ImageGalleryApi.Admin");
            //    options.Scope.Add("offline_access");
            //    //options.Scope.Add("https://storage.azure.com/user_impersonation");
            //    options.Scope.Add("https://management.azure.com/.default");
            //    options.CallbackPath = "/auth/signin-callback";
            //    options.SignedOutRedirectUri = "http://localhost:54295";
            //    options.SaveTokens = true;
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //    {
            //        NameClaimType = JwtClaimTypes.GivenName,
            //        RoleClaimType = "roles"
            //    };
            //    options.Events = new OpenIdConnectEvents()
            //    {
            //        OnAuthorizationCodeReceived = code =>
            //        {

            //            return Task.FromResult(0);
            //        },
            //        OnRedirectToIdentityProvider = context =>
            //        {
            //            return Task.FromResult(0);
            //        },
            //        OnTokenResponseReceived = token =>
            //        {
            //            return Task.FromResult(0);
            //        }


            //    };
            //})
            //    .AddCookie();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
               .AddCookie()
               .AddGoogle(options =>
               {
                   options.ClientId = Configuration["Google:ClientId"];
                   options.ClientSecret = Configuration["Google:ClientSecret"];
                   options.CallbackPath = "/CookieAuthenticationDemo/signin-google";
                   options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
                   {
                       OnTicketReceived = ticket =>
                       {
                           return Task.FromResult(0);
                       }
                   };
               });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
