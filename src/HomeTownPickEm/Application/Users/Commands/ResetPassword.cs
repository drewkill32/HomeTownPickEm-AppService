#region

using System.Text;
using System.Web;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

#endregion

namespace HomeTownPickEm.Application.Users.Commands
{
    public class ResetPassword
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly HttpContext _context;
            private readonly IEmailSender _emailSender;
            private readonly ILogger<Handler> _logger;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
                IEmailSender emailSender, ILogger<Handler> logger)
            {
                _userManager = userManager;
                _context = contextAccessor.HttpContext ?? throw new NullReferenceException("There is no HttpContext");
                _emailSender = emailSender;
                _logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogError("Attempt to reset password for user with {Email} that does not exist",
                        request.Email);
                    return Unit.Value;
                }
                
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url =
                    $"{_context.Request.Scheme}://{_context.Request.Host}/confirm-reset-password?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}";

                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to reset your password. If you did not request a password reset please ignore this email.";

                await _emailSender.SendEmailAsync(user.Email, "St. Pete Pick'em Reset Password", htmlMessage);

                return Unit.Value;
            }
        }
    }
}