using System.Security.Claims;

namespace JobInterviewProject.Infrastructure.Services.Service_Interfaces
{
    public interface IAuthorizedUserService
    {
        ClaimsPrincipal GetAuthorizedUser();
        string GetCurrentUserEmail();
        Guid GetCurrentUserId();
        bool IsAuthorized();
    }
}