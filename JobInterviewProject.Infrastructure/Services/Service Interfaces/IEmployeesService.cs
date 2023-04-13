using JobInterviewProject.Domain.Models;
using JobInterviewProject.Infrastructure.DTOs;

namespace JobInterviewProject.Infrastructure.Services.Service_Interfaces
{
    public interface IEmployeesService
    {
        Task<Employee> GetEmployeeAsync(string personalId);
        Task<List<EditEmployeeDto>> GetAllEmployees();
        Task AddEmployee(EditEmployeeDto dto);
        Task EditEmployee(EditEmployeeDto dto);
        Task RemoveEmployee(string id);
    }
}
