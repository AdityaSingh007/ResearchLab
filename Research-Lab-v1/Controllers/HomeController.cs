using Research_Lab_v1.Interfaces;
using Research_Lab_v1.Models;
using Research_Lab_v1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Research_Lab_v1.Controllers
{
    public class HomeController : Controller
    {
        ICustomerRepository _Repository;

        public HomeController()
        {
            _Repository = new CustomerRepository();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Jquery()
        {
            //return View(@"~\Views\Pages\Home.cshtml");
            return View("Knockout");
        }
        public ActionResult Cars()
        {
            return View(@"~\Views\Pages\Cars.cshtml");
        }
        public ActionResult Drones()
        {
            return View(@"~\Views\Pages\Drones.cshtml");
        }
        public ActionResult Maps()
        {
            return View(@"~\Views\Pages\Maps.cshtml");
        }

        [HttpGet]
        public ActionResult GetCustomersData()
        {
            var custs = _Repository.GetCustomers();
            return Json(custs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Orders(int custID)
        {
            var orders = _Repository.GetOrders(custID);
            Thread.Sleep(5000);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Knockout()
        {
            //return View(@"~\Views\Knockout\KoComputed.cshtml");
            //return View(@"~\Views\Knockout\ObservableArrays.cshtml");
            //return View(@"~\Views\Knockout\SubcribeChanges.cshtml");
            //return View(@"~\Views\Knockout\ControlFlow.cshtml");
            //return View(@"~\Views\Knockout\DynamicallyAssignedTemplates.cshtml");
            //return View(@"~\Views\Knockout\ExternalTemplate.cshtml");
            //return View(@"~\Views\Knockout\CustomBindingHandler.cshtml");
            //return View(@"~\Views\Knockout\ShoppingCart.cshtml");
            //return View(@"~\Views\Knockout\PapaKnockout.cshtml");
            //return View(@"~\Views\Knockout\EventHandler.cshtml");
            return View(@"~\Views\Knockout\Throttle.cshtml");
        }
        [HttpGet]
        public ActionResult ResolvePromises()
        {
            return Json(new { Name = "AdityaSingh", age = 27 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MepCharts()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GithubNamShub()
        {
            return View("GithubNamshub");
        }

        [HttpGet]
        public string ThrottlingHandler()
    {
            return "ok";
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}