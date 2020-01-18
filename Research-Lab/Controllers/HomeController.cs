using Research_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Research_Lab.Controllers
{
    public class HomeController : Controller
    {
        private string[] _courses = {
                "C#",
                "Entity Framework",
                "LINQ",
                "ASP.NET MVC",
                "jQuery UI",
                "CSS",
                "Ruby",
                "jquery",
                "JavaScript",
                "TDD",
                "Design Patterns"
        };

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

        [HttpGet]
        public ActionResult ProcessAjaxRequest(Customer customer)
        {
            return View();

        }

        //[HttpPost]
        //public ActionResult ProcessAjaxRequest(Customer customer)
        //{
        //    return new JsonResult();

        //}
        public ActionResult JqueryResearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string prefix)
        {
            var courses = _courses.Where(x => x.ToLower().Contains(prefix))
                                  .Take(4);
            return Json(courses, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Forms()
        {
            return View();
        }

        public ActionResult BindingForms()
        {
            return View();
        }

        public ActionResult FormProcessing()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FormProcessing(TestData processData)
        {
            //throw new Exception();
            return View();
        }
    }
}