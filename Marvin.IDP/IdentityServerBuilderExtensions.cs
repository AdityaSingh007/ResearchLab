using Marvin.IDP.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marvin.IDP
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddMarvinUserStore(this IIdentityServerBuilder builder)
        {
            ///**Repository should be registered as a scoped service since dbContext has been registered as a scoped service which cannot be consumed from singleton**  
            builder.Services.AddScoped<IMarvinUserRepository, MarvinUserRepository>();
            builder.AddProfileService<MarvinUserProfileService>();
            return builder;
        }
    }
}
