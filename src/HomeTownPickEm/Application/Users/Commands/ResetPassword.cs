#region

using System.Text;
using System.Web;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

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
            private readonly OriginOptions _opt;

            public Handler(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
                IOptions<OriginOptions> options,
                IEmailSender emailSender, ILogger<Handler> logger)
            {
                _userManager = userManager;
                _context = contextAccessor.HttpContext ?? throw new NullReferenceException("There is no HttpContext");
                _emailSender = emailSender;
                _logger = logger;
                _opt = options.Value;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogError("Attempt to reset password for user with {Email} that does not exist",
                        request.Email);
                    return;
                }

                var origin = _context.Request.Headers["Origin"].ToString();
                _opt.ValidateOrigin(origin);
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url =
                    $"{origin}/confirm-reset-password?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}";

                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to reset your password. If you did not request a password reset please ignore this email.";

                await _emailSender.SendEmailAsync(user.Email, "St. Pete Pick'em Reset Password", htmlMessage);
                
            }
        }
    }
}