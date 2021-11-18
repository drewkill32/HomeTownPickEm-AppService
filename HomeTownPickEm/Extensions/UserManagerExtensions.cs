using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<UserDto> CreateUserAsync(this UserManager<ApplicationUser> userManager,
            AddUserCommand user)
        {
            return (await CreateUsersAsync(userManager, new[] { user })).SingleOrDefault();
        }

        public static async Task<IEnumerable<UserDto>> CreateUsersAsync(this UserManager<ApplicationUser> userManager,
            IEnumerable<AddUserCommand> request)
        {
            var users = request.Select(x => x.ToAppUser()).ToArray();
            var results = await Task.WhenAll(users.Select(userManager.CreateAsync));
            var errors = results.SelectMany(x => x.Errors).ToArray();
            if (errors.Any())
            {
                throw new Exception(string.Join(". ", errors.Select(x => x.Description)));
            }

            return users.Select(x => x.ToUserDto());
        }
    }
}