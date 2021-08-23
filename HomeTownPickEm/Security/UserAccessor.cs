using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Http;
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

        public string GetCurrentUsername()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return username;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userName = GetCurrentUsername();
            if (string.IsNullOrEmpty(userName))
            {
                throw new ForbiddenAccessException();
            }

            return await _userManager.FindByNameAsync(userName);
        }
    }

    public interface IUserAccessor
    {
        string GetCurrentUsername();
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}