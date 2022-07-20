using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> CreateUserAsync(this UserManager<ApplicationUser> userManager,
            AddUser.Command user)
        {
            return (await CreateUsersAsync(userManager, new[] { user })).SingleOrDefault();
        }

        public static async Task<IEnumerable<ApplicationUser>> CreateUsersAsync(
            this UserManager<ApplicationUser> userManager,
            IEnumerable<AddUser.Command> request)
        {
            var users = request.Select(x => x.ToAppUser()).ToArray();
            var results = await Task.WhenAll(users.Select(userManager.CreateAsync));
            var errors = results.SelectMany(x => x.Errors).ToArray();
            if (errors.Any())
            {
                throw new Exception(string.Join(". ", errors.Select(x => x.Description)));
            }

            return users;
        }
    }
}