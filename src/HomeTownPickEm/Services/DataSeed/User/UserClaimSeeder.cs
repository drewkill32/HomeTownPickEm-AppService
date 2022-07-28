using System.Security.Claims;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services.DataSeed.User
{
    public class UserClaimSeeder : ISeeder
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserClaimSeeder(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            var email = _config.GetSection("User")["Email"];
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email,
                cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"There is no user with the email <{email}>");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == Claims.Values.Admin))
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, Claims.Values.Admin));
            }

            await RemoveOldClaims(user, claims);

            await AddCommissionerClaims(cancellationToken, claims, user);
        }

        private async Task AddCommissionerClaims(CancellationToken cancellationToken, IList<Claim> claims,
            ApplicationUser user)
        {
            var leagueIds = await _context.League.Select(x => x.Id).ToArrayAsync(cancellationToken);
            foreach (var id in leagueIds)
            {
                if (!claims.Any(x => x.Type == Claims.Types.Commissioner && x.Value == id.ToString()))
                {
                    await _userManager.AddClaimAsync(user, new Claim(Claims.Types.Commissioner, id.ToString()));
                }
            }
        }

        private async Task RemoveOldClaims(ApplicationUser user, IList<Claim> claims)
        {
            var claimsToRemove = claims
                .Where(c => c.Type == ClaimTypes.Role &&
                            c.Value.StartsWith("commissioner:",
                                StringComparison.OrdinalIgnoreCase))
                .ToArray();
            await _userManager.RemoveClaimsAsync(user, claimsToRemove);
        }
    }
}