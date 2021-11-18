using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace HomeTownPickEm.Swagger
{
    public class SwaggerAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsSwaggerPage(context) && !UserCookieExists(context))
            {
                var uri = HttpUtility.UrlEncode(
                    $"{context.Request.Scheme}://{context.Request.Host}/swagger/index.html");
                context.Response.Redirect($"/login?redirectUri={uri}");
                return;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private static bool IsSwaggerPage(HttpContext context)
        {
            return context.Request.Path.Value.Contains("/swagger/index.html");
        }

        private static bool UserCookieExists(HttpContext context)
        {
            return context.Request.Cookies.ContainsKey("user");
        }
    }
}