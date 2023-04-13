using JobInterviewProject.Api.Models;
using JobInterviewProject.Domain.Exceptions;
using JobInterviewProject.Infrastructure.DTOs;
using JobInterviewProject.Infrastructure.Services.Service_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobInterviewProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeesService.GetAllEmployees();

            return Ok(employees);
        }

        [HttpGet("{personalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllEmployees(string personalId)
        {
            if (string.IsNullOrWhiteSpace(personalId))
                return BadRequest("Personal ID can't be null.");
            
            if (personalId.Length != 11)
                return BadRequest("Personal ID Must be 11 characters long.");

            var employees = await _employeesService.GetEmployeeAsync(personalId);

            if (employees == null) 
                return NotFound();

            return Ok(employees);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddEmployee([FromBody] EditEmployeeDto model)
        {
            try
            {
                await _employeesService.AddEmployee(model);
            }
            catch (PersonalIdAlreadyExistsException ex)
            {
                return Ok(new AddEmployeeResponse { PersonalId = null, Success = false, ErrorMessage = ex.Message });
            }

            return Ok(new AddEmployeeResponse { PersonalId = model.PersonalId, Success = true, ErrorMessage = null });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditEmployee([FromBody] EditEmployeeDto model)
        {
            await _employeesService.EditEmployee(model);

            return Ok(model.PersonalId);
        }

        [HttpDelete("{personalId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RemoveEmployee(string personalId)
        {
            if (string.IsNullOrWhiteSpace(personalId))
                return BadRequest("Personal ID can't be null.");
            
            else if (personalId.Length != 11)
                return BadRequest("Personal ID Must be 11 characters long.");

            await _employeesService.RemoveEmployee(personalId);

            return NoContent();
        }
    }
}
