namespace JobInterviewProject.MVC.Services
{
    public interface ICookieService
    {
        string GetAuthorizationToken(HttpRequest request);

        void SetAuthorizationCookie(HttpResponse response, string authorizationToken, DateTimeOffset expiresIn);
    }
}
