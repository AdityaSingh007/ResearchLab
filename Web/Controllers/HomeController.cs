using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Clients;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Manual setup for Authorization code flow .Please refer startup.cs for middleware set up
            //if (User.Identity.IsAuthenticated)
            //{
            //    var accessToken = User as ClaimsPrincipal;

            //    return Content(accessToken.FindFirst("access_token").Value);
            //}
            //var url = "http://localhost:58457/connect/authorize"
            //          + "?client_id=scoialnetwork_code"
            //          + "&redirect_uri=http://localhost:41109/Home/AuthorizationCallback"
            //          + "&response_type=code"
            //          + "&scope=openid+profile+Read"
            //          + "&response_mode=form_post";

            //return Redirect(url);
            return View();
        }

        public ActionResult AuthorizationCallback(string code, string state, string error)
        {
            //Manual setup for Authorization code flow .Please refer startup.cs for middleware set up
            var tokenUrl = "http://localhost:58457/connect/token";
            var client = new OAuth2Client(new Uri(tokenUrl), "scoialnetwork_code", "secret");
            var requestResult = client.RequestAccessTokenCode(code, new Uri("http://localhost:41109/Home/AuthorizationCallback"));

            var claims = new[]
            {
                new Claim("access_token",requestResult.AccessToken)
            };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            Request.GetOwinContext().Authentication.SignIn(identity);
            return Redirect("/");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public async Task<ActionResult> Private()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", claimsPrincipal.FindFirst("access_token").Value);
            //    var profile = await client.GetAsync("http://localhost:60723/api/profiles");
            //    var profileContent = profile.Content.ReadAsStringAsync();
            //}

            return View(claimsPrincipal.Claims);
        }

        public ActionResult RefreshToken()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            var client = new OAuth2Client(new Uri("http://localhost:58457/connect/token"), "scoialnetwork_code", "secret");
            var requestResponse = client.RequestAccessTokenRefreshToken(claimsPrincipal.FindFirst("refresh_token").Value);
            var manager = HttpContext.GetOwinContext().Authentication;
            var refreshedIdentity = new ClaimsIdentity(User.Identity);
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("access_token"));
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("refresh_token"));

            refreshedIdentity.AddClaim(new Claim("access_token", requestResponse.AccessToken));
            refreshedIdentity.AddClaim(new Claim("refresh_token", requestResponse.RefreshToken));

            manager.AuthenticationResponseGrant = new Microsoft.Owin.Security.AuthenticationResponseGrant(new ClaimsPrincipal(refreshedIdentity),
                new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = true });

            return Redirect("/Private");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var client = new OAuth2Client(new Uri("http://localhost:58457/connect/token"), "camps", "secret");
            var requestResponse = client.RequestAccessTokenUserName(username, password, "openid profile Read offline_access");
            var claims = new[]
            {
                new Claim("access_token",requestResponse.AccessToken),
                new Claim("refresh_token",requestResponse.RefreshToken)
            };
            var claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(claimsIdentity);

            return Redirect("/Private");
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult AnalyticsRequest()
        {
            return View();
        }

        [HttpGet]
        public JsonResult IsDasRequestNameAlreadyTaken(int analyticsId)
        {
            var response = "true";
            if (analyticsId == 007)
                response = "false";

            Thread.Sleep(5000);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertUpdateAnalytics()
        {
            Thread.Sleep(5000);
            return Json("Success");
        }

        [HttpPost]
        public JsonResult UploadCIMFile()
        {
            Thread.Sleep(5000);
            return Json("Success");
        }

        [HttpPost]
        public JsonResult SendAcknowledgementEmail()
        {
            Thread.Sleep(5000);
            return Json("Success");
        }

    }
}