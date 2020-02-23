using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AzureADAuth.Models;
using AzureADAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.WindowsAzure.Storage;

namespace AzureADAuth.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly ImageStore imageStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImageController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var currentContext = _httpContextAccessor.HttpContext;
            var accessToken = currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
            this.imageStore = new ImageStore(accessToken);
            //this.imageStore = new ImageStore();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    using (var stream = image.OpenReadStream())
                    {
                        var imageId = await imageStore.SaveImage(stream);
                        return RedirectToAction("Show", new { imageId });
                    }
                }
            }
            catch (StorageException ex)
            {
                //if(ex.RequestInformation.HttpStatusCode==(int)HttpStatusCode.Forbidden)

            }
            catch (Exception)
            {

                throw;
            }
           
            return View();
        }

        [HttpGet("{imageId}")]
        public ActionResult Show(string imageId)
        {
            var model = new ShowModel { Uri = imageStore.UriFor(imageId) };
            return View(model);
        }
    }
}