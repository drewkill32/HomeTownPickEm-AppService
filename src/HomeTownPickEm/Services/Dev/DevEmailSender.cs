using HomeTownPickEm.Application.Common;

namespace HomeTownPickEm.Services.Dev;

public class DevEmailSender : IEmailSender
{
    private readonly ILogger<DevEmailSender> _logger;

    public DevEmailSender(ILogger<DevEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var file = $"{Path.GetTempFileName()}.html";
        File.WriteAllText(file, htmlMessage);
        _logger.LogInformation("Sending email to: {Email}. {Subject}, {Path}", email, subject, file);
        return Task.CompletedTask;
    }
}