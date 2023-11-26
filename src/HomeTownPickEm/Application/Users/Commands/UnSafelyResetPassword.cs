using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class UnSafelyResetPassword
    {
        public class Command : IRequest
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string Secret { get; set; }
        }


        public class CommandHandler : IRequestHandler<Command>
        {
            private const string Secret = "This Is My Secret Key. GREG!";
            private readonly UserManager<ApplicationUser> _userManager;


            public CommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Secret != Secret)
                {
                    throw new ForbiddenAccessException("Invalid login");
                }

                var user = (await _userManager.FindByEmailAsync(request.Email))
                    .GuardAgainstNotFound("No User Found");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var response = await _userManager.ResetPasswordAsync(user, code, request.Password);

                if (response.Succeeded)
                {
                    return;
                }

                throw new Exception(string.Join(".", response.Errors.Select(x => x.Description)));
            }
        }
    }
}