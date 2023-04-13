using JobInterviewProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobInterviewProject.Domain.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        public string? PersonalId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender? UserGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Position { get; set; }
        public Status EmployeeStatus { get; set; }
        public DateTime? DateOfFire { get; set; }
    }
}
