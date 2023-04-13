using JobInterviewProject.MVC.Models;
using JobInterviewProject.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobInterviewProject.MVC.Controllers
{
    [Route("/[Controller]")]
    public class EmployeesController : Controller
    {
        private readonly IHttpClientService _http;
        private readonly ICookieService _cookieService;
        private readonly IConfiguration _configuration;

        public EmployeesController(IHttpClientService http, ICookieService cookieService, IConfiguration configuration)
        {
            _http = http;
            _cookieService = cookieService;
            _configuration = configuration;
        }

        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var token = _cookieService.GetAuthorizationToken(Request);

            var employees = await _http.GetAsync<List<EmployeeViewModel>>($"{_configuration["Url:DefaultUrl"]}api/employees", token);

            return View("GetAll", employees);
        }

        [HttpGet, Route("AddEmployee")]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost, Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromForm] EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            var token = _cookieService.GetAuthorizationToken(Request);

            var result = await _http.PostAsync<AddEmployeeResponse>($"{_configuration["Url:DefaultUrl"]}api/employees", model, token);

            if (!result.Success)
            {
                return View("PersonalIdExistsError", new ErrorViewModel() { ErrorMessage = result.ErrorMessage});
            }

            return RedirectToAction("GetAll");
        }


        [HttpGet, Route("Edit/{personalId}")]
        public async Task<IActionResult> EditEmployee(string personalId)
        {
            var token = _cookieService.GetAuthorizationToken(Request);

            var employee = await _http.GetAsync<EditEmployeeViewModel>($"{_configuration["Url:DefaultUrl"]}api/employees/{personalId}", token);

            if (employee == null)
                return View("Error");

            return View(employee);
        }


        [HttpPost, Route("Edit")]
        public async Task<IActionResult> EditEmployee([FromForm] EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            var token = _cookieService.GetAuthorizationToken(Request);

            await _http.PutAsync<string>($"{_configuration["Url:DefaultUrl"]}api/employees", model, token);

            return RedirectToAction("GetAll");
        }

        [Route("Remove/{personalId}")]
        public async Task<IActionResult> RemoveEmployee(string personalId)
        {
            if (personalId == null)
                return View("Error");
            
            if (personalId.Length != 11)
                return View("Error");

            var token = _cookieService.GetAuthorizationToken(Request);

            await _http.DeleteAsync($"{_configuration["Url:DefaultUrl"]}api/employees/{personalId}", token);

            return RedirectToAction("GetAll");
        }
    }
}
