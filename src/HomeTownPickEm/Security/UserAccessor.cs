using System.Security.Claims;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAccessor(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetCurrentUserId()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return username;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var id = GetCurrentUserId();
            if (string.IsNullOrEmpty(id))
            {
                throw new ForbiddenAccessException();
            }

            return await _userManager.FindByIdAsync(id);
        }
    }

    public interface IUserAccessor
    {
        string GetCurrentUserId();
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}