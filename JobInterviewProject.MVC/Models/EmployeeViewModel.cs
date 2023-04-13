using System.ComponentModel.DataAnnotations;

namespace JobInterviewProject.MVC.Models
{
    public class EmployeeViewModel
    {
        [MinLength(11), MaxLength(11)]
        public string? PersonalId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public Gender? UserGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public Status EmployeeStatus { get; set; }
        public DateTime? DateOfFire { get; set; }
    }
}
