namespace JobInterviewProject.MVC.Middlewares
{
    public class AuthorizationTokenMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cookie = context.Request.Cookies["Authorization"];

            if (string.IsNullOrWhiteSpace(cookie))
            {
                await next(context);
                return;
            }

            context.Request.Headers.Add("Authorization", $"Bearer {cookie}");
            await next(context);
        }
    }
}
