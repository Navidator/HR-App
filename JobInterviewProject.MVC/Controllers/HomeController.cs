using JobInterviewProject.MVC.Models;
using JobInterviewProject.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobInterviewProject.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICookieService _cookieService;

        public HomeController(ICookieService cookieService)
        {
            _cookieService= cookieService;
        }

        public IActionResult Index()
        {
            var token = _cookieService.GetAuthorizationToken(Request);

            return !string.IsNullOrWhiteSpace(token) ? RedirectToAction("GetAll", "Employees") : View();
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