using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ImageGallery.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers
{
    [Produces("application/json")]
    public class AzureController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public AzureController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        [HttpGet()]
        [Authorize(Policy = "UserMustBeAnAdmin")]
        [Route("api/Azure/GetAzure")]
        public IActionResult GetAzure()
        {
            var signedInUser = User.Identities;
            return Ok("Token verification successful");
        }

        [HttpPost]
        [Route("api/Azure/AuthorizeUsingCustomModel")]
        public async Task<IActionResult> AuthorizeUsingCustomModel([FromBody]Profile profile)
        {
            /*Demo code for performing resource based auhthorization.Allows user to perfom auhthorization by passing in a resource model*/
            var authResult = await _authorizationService.AuthorizeAsync(User, profile, "IsProfileValid");
            if (authResult.Succeeded)
                return Ok(new { status = HttpStatusCode.OK, data = "Authorization Successfull", Model = profile });
            else
                return Unauthorized();
        }
    }
}