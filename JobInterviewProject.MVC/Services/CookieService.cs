namespace JobInterviewProject.MVC.Services
{
    public class CookieService : ICookieService
    {
        public void SetAuthorizationCookie(HttpResponse response, string authorizationToken, DateTimeOffset expiresIn)
        {
            response.Cookies.Append("Authorization", $"Bearer {authorizationToken}", GetCookieOptions(expiresIn));
        }

        public string GetAuthorizationToken(HttpRequest request) => request.Cookies["Authorization"];

        private CookieOptions GetCookieOptions(DateTimeOffset expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = expires
            };
            return cookieOptions;
        }
    }
}
