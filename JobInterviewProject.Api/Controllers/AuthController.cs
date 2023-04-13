using JobInterviewProject.Infrastructure.DTOs;
using JobInterviewProject.Infrastructure.Services.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobInterviewProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _authService.Login(model);

            if (user != null)
            {
                return Ok(user);
            }

            return Unauthorized("User could not be authorized");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            await _authService.Register(model);

            var user = await _authService.Login(new LoginDto { Email = model.Email, Password = model.Password });

            return Ok(user);
        }
    }
}
