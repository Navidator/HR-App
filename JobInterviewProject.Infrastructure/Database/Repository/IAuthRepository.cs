using JobInterviewProject.Domain.Models;

namespace JobInterviewProject.Infrastructure.Database.Repository
{
    public interface IAuthRepository
    {
        Task<User> Login(string email);
        Task<User> Register(User registerUser);
        Task AddRefreshTokenAsync(RefreshTokenModel refreshToken);
        Task<RefreshTokenModel> GetRefreshTokenAsync(string token);
    }
}
