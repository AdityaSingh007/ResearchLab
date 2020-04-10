using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AzureCompute.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureCompute
{
    public class HomeController : Controller
    {
        private SalesContext _salesContext;
        private readonly IConfiguration _confguration;

        public HomeController(SalesContext salesContext, IConfiguration configuration)
        {
            _salesContext = salesContext;
            _confguration = configuration;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var customers = _salesContext.customers.Take(100);
            return Ok(customers);
        }

        [Route("DisplayMessageFromAppSettings")]
        public IActionResult DisplayMessageFromAppSettings()
        {
            var connectionString = _confguration["ConnectionStrings:salesDBConnectionString"];
            var message = _confguration["Message"];
            return Ok(new { connectionString = connectionString, message = message });
        }
    }
}
