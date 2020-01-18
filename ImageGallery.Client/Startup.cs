using IdentityModel;
using ImageGallery.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImageGallery.Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    "CanOrderFrame",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("country", "be");
                        policyBuilder.RequireClaim("subscriptionlevel", "PayingUser");
                    });
            });

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register an IImageGalleryHttpClient
            services.AddScoped<IImageGalleryHttpClient, ImageGalleryHttpClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", (options) =>
             {
                 options.AccessDeniedPath = "/Authorization/AccessDenied";
             })
              .AddOpenIdConnect("oidc", options =>
              {
                  options.SignInScheme = "Cookies";
                  options.Authority = "http://localhost:54248/";
                  options.ClientId = "imagegalleryclient";
                  options.ResponseType = "code id_token";
                  //options.CallbackPath = new PathString("...")
                  //options.SignedOutCallbackPath = new PathString("...")
                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  options.Scope.Add("address");
                  options.Scope.Add("role");
                  options.Scope.Add("offline_access");
                  options.Scope.Add("imagegalleryapi");
                  options.Scope.Add("subscriptionlevel");
                  options.Scope.Add("country");
                  options.SaveTokens = true;
                  options.ClientSecret = "secret";
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.RequireHttpsMetadata = false;
                  options.ClaimActions.Remove("amr");
                  options.ClaimActions.DeleteClaim("sid");
                  options.ClaimActions.DeleteClaim("idp");
                  ///If below statement is excluded then role claim will not be mapped in claims collection.
                  options.ClaimActions.MapUniqueJsonKey("role", "role");
                  options.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");
                  options.ClaimActions.MapUniqueJsonKey("country", "country");
                  ///If below statement is excluded then mapped role claim will not be used in RBAC.
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                  {
                      NameClaimType = JwtClaimTypes.GivenName,
                      RoleClaimType = JwtClaimTypes.Role
                  };
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}
