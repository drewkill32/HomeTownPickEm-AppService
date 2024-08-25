using System.Text;
using System.Web;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Services;

public class EmailTemplateFactory
{
    private readonly HttpContext _context;
    private readonly OriginOptions _origin;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmailTemplateFactory(IHttpContextAccessor accessor,
        IOptions<OriginOptions> originOptions, UserManager<ApplicationUser> userManager)
    {
        _context = accessor.HttpContext;
        _origin = originOptions.Value;
        _userManager = userManager;
    }

    public async Task<EmailTemplate> CreateEmailTemplate(EmailType emailType, ApplicationUser user)
    {
        var origin = _context.Request.Headers["Origin"].ToString();
        _origin.ValidateOrigin(origin);

        switch (emailType)
        {
            case EmailType.NewUser:
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url =
                    $"{origin}/new-user?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}&firstName={user.Name.First}&lastName={user.Name.Last}";

                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to confirm your email and join the league. If you did not generate this request ignore this email.";
                return new()
                {
                    Subject = "You have been invited to join a league at St. Pete Pick'em",
                    HtmlMessage = htmlMessage
                };
            }
            case EmailType.Register:
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url =
                    $"{origin}/confirm-email?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}";

                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to confirm your email. If you did not generate this request ignore this email.";
                return new()
                {
                    Subject = "St. Pete Pick'em Register",
                    HtmlMessage = htmlMessage
                };
            }
            case EmailType.ForgotPassword:
            {
                var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordCode));
                var url =
                    $"{origin}/confirm-reset-password?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}";
                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to reset your password. If you did not request a password reset please ignore this email.";
                return new()
                {
                    Subject = "St. Pete Pick'em Reset Password",
                    HtmlMessage = htmlMessage
                };
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(emailType), emailType, null);
        }
    }
}

public struct EmailTemplate
{
    public string HtmlMessage { get; set; }
    public string Subject { get; set; }
}