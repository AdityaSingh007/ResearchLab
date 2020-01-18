using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace RestFullServices.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage getNavObjects(bool isHtmlApp)
        {
            var navObjects = new List<dynamic>();
            if (isHtmlApp)
            {
                navObjects.Add(new { title = "Home", src = "../AppPages/Home.html" });
                navObjects.Add(new { title = "Cars", src = "../AppPages/Cars.html" });
                navObjects.Add(new { title = "Drones", src = "../AppPages/Drones.html" });
                navObjects.Add(new { title = "Map", src = "../AppPages/Maps.html" });

            }
            else
            {
                navObjects.Add(new { title = "Home", src = "/Home/Jquery" });
                navObjects.Add(new { title = "Cars", src = "/Home/Cars" });
                navObjects.Add(new { title = "Drones", src = "/Home/Drones" });
                navObjects.Add(new { title = "Map", src = "/Home/Maps" });

            }
            return Request.CreateResponse(HttpStatusCode.OK, navObjects);

        }

        [HttpGet]
        [Authorize]
        [Route("values/GetLocation")]
        public HttpResponseMessage GetLocation()
        {
            HttpResponseMessage httpResponseMessage;
            try
            {
                var response = new
                {
                    Latitude = 10,
                    Longitude = 20,
                    IsAdmin = User.IsInRole("Admin"),
                    UserName = ClaimsPrincipal.Current.Identity.Name,
                    claims = ClaimsPrincipal.Current.Claims
                };

                //if (ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/scope").Value != "user_impersonation")
                //{
                //    throw new HttpResponseException(new HttpResponseMessage
                //    {
                //        StatusCode = HttpStatusCode.Unauthorized,
                //        ReasonPhrase = "The Scope claim does not contain 'user_impersonation' or scope claim not found"
                //    });
                //}

                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = response });
            }
            catch (Exception ex)
            {
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }

            return httpResponseMessage;
        }
    }
}
