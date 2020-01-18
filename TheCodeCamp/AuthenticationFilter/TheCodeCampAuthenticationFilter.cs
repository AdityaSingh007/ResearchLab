using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace TheCodeCamp.AuthenticationFilter
{
    public class TheCodeCampAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"AuthenicationFilter(Name) => {System.Security.Principal.WindowsIdentity.GetCurrent().Name}");
            Debug.WriteLine("**-------------**");
            Debug.WriteLine($"AuthenicationFilter(IsAutheticated) => {System.Security.Principal.WindowsIdentity.GetCurrent().IsAuthenticated}");
            Debug.WriteLine("**-------------**");
            Debug.WriteLine($"AuthenicationFilter(Principal) => {context.ActionContext.RequestContext.Principal}");
            if(System.Security.Principal.WindowsIdentity.GetCurrent().IsAuthenticated)
            context.Principal = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {

        }
    }
}