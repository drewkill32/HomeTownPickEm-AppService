using System.Text;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace HomeTownPickEm.Application.Users.Commands;

public class VerifyEmail
{
    public class Command : IRequest
    {
        public string Email { get; set; }

        public string Code { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<VerifyEmail> _logger;

        public CommandHandler(UserManager<ApplicationUser> userManager, ILogger<VerifyEmail> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogError("User with email {Email} not found.", request.Email);
                throw new NotFoundException($"Unable to load user with email '{request.Email}'");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                _logger.LogInformation("Confirm email for {Email}", request.Email);
                return Unit.Value;
            }

            _logger.LogError("Unable to verify user confirm email token for user '{Email}'. {Errors}", request.Email,
                string.Join(", ", result.Errors.Select(x => x.Description)));

            throw new BadRequestException("Invalid token");
        }
    }
}