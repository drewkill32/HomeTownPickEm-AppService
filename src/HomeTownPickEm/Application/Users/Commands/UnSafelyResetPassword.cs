using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands;

public class UnSafelyResetPassword
{
    public class Command : IRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }


    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IUserAccessor _userAccessor;


        public CommandHandler(UserManager<ApplicationUser> userManager, IConfiguration config,
            IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _config = config;
            _userAccessor = userAccessor;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var principal = _userAccessor.GetClaimsPrincipal();
            if (!principal.Claims.Any(c => c is { Type: Claims.Types.Admin, Value: Claims.Values.True }))
            {
                throw new ForbiddenAccessException("User must be an admin");
            }

            var user = (await _userManager.FindByEmailAsync(request.Email))
                .GuardAgainstNotFound("No User Found");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = await _userManager.ResetPasswordAsync(user, code, request.Password);

            if (response.Succeeded)
            {
                return;
            }

            throw new(string.Join(".", response.Errors.Select(x => x.Description)));
        }
    }
}