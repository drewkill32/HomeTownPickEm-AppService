using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Users;
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

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            if (IsSwaggerPage(context))
            {
                if (!UserCookieExists(context))
                {
                    var uri = HttpUtility.UrlEncode(
                        $"{context.Request.Scheme}://{context.Request.Host}/swagger/index.html");
                    context.Response.Redirect($"/login?redirectUri={uri}");
                    return;
                }

                if (!IsAdmin(context, jwtService))
                {
                    var uri = HttpUtility.UrlEncode(
                        $"{context.Request.Scheme}://{context.Request.Host}/swagger/index.html");
                    context.Response.Redirect($"/unauthorized?redirectUri={uri}&pageTitle=Hometown%20API");
                    return;
                }
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private static bool IsAdmin(HttpContext context, IJwtService jwtService)
        {
            var userJson = context.Request.Cookies["user"]
                           ?? throw new NullReferenceException("There is no user cookie defined.");

            var user = JsonSerializer.Deserialize<UserDto>(userJson,
                           new JsonSerializerOptions(JsonSerializerDefaults.Web))
                       ?? throw new Exception($"Unable to deserialize user from '{userJson}'");

            var token = user.Token
                        ?? throw new NullReferenceException($"There is no JWT token for the user {user.Id}");

            var claims = jwtService.GetClaims(token);
            return claims.Any(x => x.Type == ClaimTypes.Role && x.Value.ToLower() == "admin");
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