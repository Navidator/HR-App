using JobInterviewProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JobInterviewProject.Infrastructure.Database.Repository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _context;
        public EmployeesRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Employee>> GetAllEmployees()
        {
            return _context.Employees.ToListAsync();
        }

        public async Task AddEmployee(Employee employee)
        {
            var employeeToAdd = await _context.Employees.Where(x => x.PersonalId == employee.PersonalId || x.Email == employee.Email).FirstOrDefaultAsync();
            if (employeeToAdd == null)
            {
                await _context.Employees.AddAsync(employee);
            }
            else if (employeeToAdd != null)
            {
                throw new Exception("Employee already exists.");
            }
        }

        public async Task RemoveEmployee(string id)
        {
            var employeeToRemove = await _context.Employees.Where(x => x.PersonalId == id).FirstOrDefaultAsync();

            if (employeeToRemove == null)
            {
                throw new Exception("Employee Doesn't exist.");
            }
            _context.Employees.Remove(employeeToRemove);
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.PersonalId == id);
        }
    }
}
