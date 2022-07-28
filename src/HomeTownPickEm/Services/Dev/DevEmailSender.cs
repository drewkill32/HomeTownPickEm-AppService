using HomeTownPickEm.Config;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Services.Dev;

public class DevEmailSender : EmailSenderBase
{
    public DevEmailSender(IOptions<SendGridSettings> options, IHttpContextAccessor accessor,
        IOptions<OriginOptions> originOptions, UserManager<ApplicationUser> userManager,
        ILogger<SendGridEmailSender> logger) : base(options, accessor, originOptions, userManager, logger)
    {
    }

    public override Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var file = $"{Path.GetTempFileName()}.html";
        File.WriteAllText(file, htmlMessage);
        _logger.LogInformation("Sending email to: {Email}. {Subject}, {Path}", email, subject, file);
        return Task.CompletedTask;
    }
}