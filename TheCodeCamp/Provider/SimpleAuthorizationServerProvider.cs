using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TheCodeCamp.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (!string.Equals(context.UserName, context.Password, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Rejected();
                return;
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("sub", context.UserName));
            id.AddClaim(new Claim("role", "user"));
            id.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            context.Validated(id);
        }
    }
}