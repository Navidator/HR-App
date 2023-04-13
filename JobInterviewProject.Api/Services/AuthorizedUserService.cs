using JobInterviewProject.Infrastructure.Services.Service_Interfaces;
using System.Security.Claims;

namespace JobInterviewProject.Api.Services
{
    public class AuthorizedUserService : IAuthorizedUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizedUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ClaimsPrincipal GetAuthorizedUser() => _contextAccessor.HttpContext.User;

        public Guid GetCurrentUserId() =>
            Guid.Parse(_contextAccessor
                .HttpContext
                .User
                .Claims
                .First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        public string GetCurrentUserEmail() =>
            _contextAccessor
                .HttpContext
                .User
                .Claims
                .First(x => x.Type == ClaimTypes.Email).Value;

        public bool IsAuthorized()
        {
            var authorizedUser = GetAuthorizedUser();

            return authorizedUser != null && authorizedUser.Claims.Count() != 0;
        }
    }
}
