using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Utils;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services.DataSeed.User
{
    public class UserProfilePicSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public UserProfilePicSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            var usersWithoutPics = await _context.Users
                .Where(x => x.TeamId != null && string.IsNullOrEmpty(x.ProfileImg))
                .Include(x => x.Team)
                .Select(x => new { User = x, x.Team.Logos })
                .ToArrayAsync(cancellationToken);

            foreach (var userKey in usersWithoutPics)
            {
                userKey.User.ProfileImg = LogoHelper.GetSingleLogo(userKey.Logos);
                _context.Update(userKey.User);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}