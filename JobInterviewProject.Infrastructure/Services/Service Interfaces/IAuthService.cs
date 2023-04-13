using JobInterviewProject.Domain.Models;
using JobInterviewProject.Infrastructure.DTOs;

namespace JobInterviewProject.Infrastructure.Services.Service_Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> Login(LoginDto model);
        Task<User> Register(RegisterDto model);
        Task<AuthResultDto> RequestNewToken(RefreshTokenDto refreshToken);
        Task<AuthResultDto> VerifyAndGenerateTokenAsync(RefreshTokenDto refreshToken);
        Task<AuthResultDto> GenerateJWTTokenAsync(User user, RefreshTokenModel rToken);
    }
}
