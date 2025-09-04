using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Core_Project.Models;

namespace My_Core_Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration conf;

        public HomeController(ILogger<HomeController> logger,IConfiguration _conf)
        {
            _logger = logger;
            conf = _conf;
        }

        public IActionResult Index()
        {
            string str = conf.GetConnectionString("DBConnection");
            string AppName = conf.GetValue<string>("AppName");
            string Version = conf.GetValue<string>("2.0-beta");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View(); // Renders Views/Home/AccessDenied.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(); // tumhara Error.cshtml
        }

    }
}
