using JobInterviewProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobInterviewProject.Infrastructure.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? UserName { get; set; }
        public string? PersonalId { get; set; }
        public Gender? UserGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
