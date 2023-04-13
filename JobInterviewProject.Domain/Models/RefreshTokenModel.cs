using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobInterviewProject.Domain.Models
{
    public class RefreshTokenModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateExpire { get; set; }

        public string UserId { get; set; }
    }
}
