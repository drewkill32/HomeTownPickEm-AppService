using System.Security.Claims;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
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
            if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Admin"))
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
            }

            var leagueIds = await _context.League.Select(x => x.Id).ToArrayAsync(cancellationToken);
            foreach (var id in leagueIds)
            {
                if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Admin"))
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, $"Commissioner:{id}"));
                }
            }
            
        }
    }
}