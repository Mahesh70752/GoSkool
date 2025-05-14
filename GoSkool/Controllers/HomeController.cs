using System.Diagnostics;
using GoSkool.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoSkool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }else if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Teacher");
            }else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
