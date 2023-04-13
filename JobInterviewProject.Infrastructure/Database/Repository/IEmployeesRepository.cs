using JobInterviewProject.Domain.Models;

namespace JobInterviewProject.Infrastructure.Database.Repository
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>> GetAllEmployees();
        Task AddEmployee(Employee employee);
        Task RemoveEmployee(string id);
        Task<Employee> GetEmployeeById(string id);
    }
}
