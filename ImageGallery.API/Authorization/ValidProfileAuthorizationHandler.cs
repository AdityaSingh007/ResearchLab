using ImageGallery.API.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.API.Authorization
{
    public class ValidProfileAuthorizationHandler : AuthorizationHandler<ValidProfileRequirement, Profile>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidProfileRequirement requirement, Profile resource)
        {
            if (!string.IsNullOrEmpty(resource.UserName) && resource.Age >= 18)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
