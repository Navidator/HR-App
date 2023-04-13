using JobInterviewProject.Domain.Exceptions;
using JobInterviewProject.Domain.Models;
using JobInterviewProject.Infrastructure.Database.UnitOfWork;
using JobInterviewProject.Infrastructure.DTOs;
using JobInterviewProject.Infrastructure.Services.Service_Interfaces;

namespace JobInterviewProject.Infrastructure.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EditEmployeeDto>> GetAllEmployees()
        {
            var employeesFromDb = await _unitOfWork.EmployeesRepository.GetAllEmployees();

            var x = employeesFromDb.Select(x => new EditEmployeeDto()
            {
                PersonalId = x.PersonalId,
                Name = x.Name,
                LastName = x.LastName,
                Email = x.Email,
                UserGender = x.UserGender,
                DateOfBirth = x.DateOfBirth,
                Position = x.Position,
                EmployeeStatus = x.EmployeeStatus,
                DateOfFire = x.DateOfFire
            });

            return x.ToList();
        }

        public async Task AddEmployee(EditEmployeeDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var existing = await _unitOfWork.EmployeesRepository.GetEmployeeById(dto.PersonalId);

            if (existing != null)
                throw new PersonalIdAlreadyExistsException($"Personal Id: {dto.PersonalId} already exists");

            var gender = (Domain.Models.Enums.Gender)dto.UserGender;
            var status = dto.EmployeeStatus;

            Employee employee = new Employee()
            {
                PersonalId = dto.PersonalId,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                UserGender = gender,
                DateOfBirth = dto.DateOfBirth,
                Position = dto.Position,
                EmployeeStatus = status,
                DateOfFire = dto.DateOfFire
            };

            await _unitOfWork.EmployeesRepository.AddEmployee(employee);
            await _unitOfWork.Complete();
        }

        public async Task EditEmployee(EditEmployeeDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var employeeToEdit = await _unitOfWork.EmployeesRepository.GetEmployeeById(dto.PersonalId);

            if (employeeToEdit == null)
            {
                throw new Exception("Employee was not found");
            }

            var gender = dto.UserGender;
            var status = dto.EmployeeStatus;

            employeeToEdit.PersonalId = dto.PersonalId;
            employeeToEdit.Name = dto.Name;
            employeeToEdit.LastName = dto.LastName;
            employeeToEdit.Email = dto.Email;
            employeeToEdit.UserGender = gender;
            employeeToEdit.DateOfBirth = dto.DateOfBirth;
            employeeToEdit.Position = dto.Position;
            employeeToEdit.EmployeeStatus = status;
            employeeToEdit.DateOfFire = dto.DateOfFire;

            await _unitOfWork.Complete();
        }

        public async Task RemoveEmployee(string id)
        {
            await _unitOfWork.EmployeesRepository.RemoveEmployee(id);
            await _unitOfWork.Complete();
        }

        public async Task<Employee> GetEmployeeAsync(string personalId)
        {
            return await _unitOfWork.EmployeesRepository.GetEmployeeById(personalId);
        }
    }
}
