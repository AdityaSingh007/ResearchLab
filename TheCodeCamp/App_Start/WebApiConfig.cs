using Microsoft.Web.Http.Versioning;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TheCodeCamp.AuthenticationFilter;

namespace TheCodeCamp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Add Authentication Filter
            //config.Filters.Add(new TheCodeCampAuthenticationFilter());

            // Web API configuration and services
            AutofacConfig.Register();

            //Versioning settings
            config.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            // Web API routes
            config.MapHttpAttributeRoutes();

            //CamelCase formatter
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Enable CORS
            config.EnableCors();
        }
    }
}
