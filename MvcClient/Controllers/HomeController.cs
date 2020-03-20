using Microsoft.Identity.Client;
using MvcClient.App_Start;
using MvcClient.Models;
using MvcClient.Services;
using MvcClient.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> AssignRole()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            var confidentialClientApplication = MSALAppBuilder.GetAppBuilder(signedInUserID, this.HttpContext);
            var user = await confidentialClientApplication.GetAccountsAsync();
            AuthenticationResult result = await confidentialClientApplication.AcquireTokenSilentAsync(new List<string>() { AuthenticationConfig.azureManagementScope },
               user.FirstOrDefault(), AuthenticationConfig.authority, false);

            var roleAssignmentModel = new RoleAssignmentModel()
            {
                SubscriptionId = "47ca3602-b986-46de-a99a-e473c26bd588",
                ResourceGroupName = "AdityaAzureRG",
                StorageAccountName = "researchstorageacct",
                ContainerName = "research",
                RoleId = "2a2b9908-6ea1-4ae2-8e65-a410df84e7d1",
                ServicePrincipalObjectId = "04f300cc-5632-4820-8fd2-9d36e7efd020",
                RoleAssignmentName = Guid.NewGuid().ToString()
            };

            var roleAssignmentService = new RoleAssignmentService(roleAssignmentModel);
            var httpResponseMessage = await roleAssignmentService.AssignRole(result.AccessToken);
            var responeFromazureAPi = await httpResponseMessage.Content.ReadAsStringAsync();
            var deserializeResponse = JsonConvert.DeserializeObject(responeFromazureAPi).ToString();

            return View("AssignRole", (object)deserializeResponse);
        }
    }
}