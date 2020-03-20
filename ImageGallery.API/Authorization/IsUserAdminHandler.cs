using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.API.Authorization
{
    public class IsUserAdminHandler : AuthorizationHandler<IsUserAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserAdminRequirement requirement)
        {
            var upn = context.User
                             .Claims
                             .Where(x => x.Type == "upn")
                             .SingleOrDefault()
                             .Value;

            if (string.Equals(upn, "jamesbond@singhadi041outlook.onmicrosoft.com", StringComparison.CurrentCultureIgnoreCase))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
