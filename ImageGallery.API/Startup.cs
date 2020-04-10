using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using ImageGallery.API.Authorization;
using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace ImageGallery.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    "MustOwnImage",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.AddRequirements(
                                new MustOwnImageRequirement());
                    });

                authorizationOptions.AddPolicy(
                    "UserMustBeAnAdmin",
                    policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddRequirements(
                        new IsUserAdminRequirement());
                });

                authorizationOptions.AddPolicy(
                    "IsProfileValid",
                    policyBuilder =>
                {
                    policyBuilder.AddRequirements(
                        new ValidProfileRequirement());
                });

            });

            ///***Configuration for authentication server as Identity Server***///
            //services.AddAuthentication(
            //    IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "http://localhost:54248/";
            //        options.ApiName = "imagegalleryapi";
            //        ///To be set only in case if token type is set to reference.
            //        //options.ApiSecret = "apisecret";
            //        options.RequireHttpsMetadata = false;
            //    });

            ///***Configuration for authentication server as Azure Ad***///
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://login.microsoftonline.com/71f2e7b5-db7d-4db7-9b7d-dc4fbc5cb0aa";
                options.Audience = "2716d103-ccb8-4499-9471-df3becb40f94";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = "roles"
                };
            });

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["ConnectionStrings:imageGalleryDBConnectionString"];
            services.AddDbContext<GalleryContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            //register custom policy Handler
            services.AddScoped<IAuthorizationHandler, MustOwnImageHandler>();
            services.AddScoped<IAuthorizationHandler, IsUserAdminHandler>();
            services.AddSingleton<IAuthorizationHandler, ValidProfileAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, GalleryContext galleryContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        // ensure generic 500 status code on fault.
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseStaticFiles();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Map from Image (entity) to Image, and back
                cfg.CreateMap<Image, Model.Image>().ReverseMap();

                // Map from ImageForCreation to Image
                // Ignore properties that shouldn't be mapped
                cfg.CreateMap<Model.ImageForCreation, Image>()
                    .ForMember(m => m.FileName, options => options.Ignore())
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.OwnerId, options => options.Ignore());

                // Map from ImageForUpdate to Image
                // ignore properties that shouldn't be mapped
                cfg.CreateMap<Model.ImageForUpdate, Image>()
                    .ForMember(m => m.FileName, options => options.Ignore())
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.OwnerId, options => options.Ignore());
            });

            AutoMapper.Mapper.AssertConfigurationIsValid();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
