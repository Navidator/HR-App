using JobInterviewProject.MVC.Models;
using JobInterviewProject.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobInterviewProject.MVC.Controllers
{
    [Route("/[controller]")]
    public class AuthController : Controller
    {
        private readonly ICookieService _cookieService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _http;

        public AuthController(ICookieService cookieService, IConfiguration configuration, IHttpClientService http)
        {
            _cookieService = cookieService;
            _configuration = configuration;
            _http = http;
        }

        [HttpGet, Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");


            var result = await _http.PostAsync<AuthResultViewModel>($"{_configuration["Url:DefaultUrl"]}api/Auth/login", model, null);

            _cookieService.SetAuthorizationCookie(Response, result.Token, result.ExpiresAt);

            return RedirectToAction("GetAll", "Employees");
        }

        [HttpGet, Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _http.PostAsync<AuthResultViewModel>($"{_configuration["Url:DefaultUrl"]}api/Auth/register", model, null);

                    _cookieService.SetAuthorizationCookie(Response, result.Token, result.ExpiresAt);

                    return RedirectToAction("GetAll", "Employees");
                }
                catch (Exception)
                {
                    return RedirectToAction("Login");
                }

            }

            if (!ModelState.IsValid)
                return View("Error");

            return View();
        }
    }
}
