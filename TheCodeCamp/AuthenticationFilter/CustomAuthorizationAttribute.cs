using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TheCodeCamp.AuthenticationFilter
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        private bool _outcome = false;
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;
            var isAdmin = actionContext.RequestContext.Principal.IsInRole("User");
            if (principal.Identity.IsAuthenticated)
                _outcome = true;
            else
                _outcome = false;
            return _outcome;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}