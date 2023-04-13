using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobInterviewProject.Domain.Models
{
    public class User : IdentityUser
    {
        public string? PersonalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        [Required]
        public override string Email { get => base.Email; set => base.Email = value; }
    }
}
