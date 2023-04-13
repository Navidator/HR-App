using System.Security.Claims;

namespace JobInterviewProject.MVC.Services
{
    public interface IAuthorizedUserService
    {
        ClaimsPrincipal GetAuthorizedUser();
        string GetCurrentUserEmail();
        Guid GetCurrentUserId();
        bool IsAuthorized();
    }
}