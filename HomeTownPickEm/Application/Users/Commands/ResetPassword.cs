#region

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

#endregion

namespace HomeTownPickEm.Application.Users.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly HttpContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IEmailSender emailSender, ILogger<ResetPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _context = contextAccessor.HttpContext ?? throw new NullReferenceException("There is no HttpContext");
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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
                $"{_context.Request.Scheme}://{_context.Request.Host}/confirmresetpassword?code={webCode}&email={user.Email}";

            var htmlMessage =
                $"Click <a href=\"{url}\">here</a> to reset your password. If you did not request a password reset please ignore this email.";

            await _emailSender.SendEmailAsync(user.Email, "St. Pete Pick'em Reset Password", htmlMessage);

            return Unit.Value;
        }
    }

    public class ResetPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }
}