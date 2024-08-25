using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Config;
using Microsoft.Extensions.Options;
using PostmarkDotNet;

namespace HomeTownPickEm.Services;

public class PostmarkEmailSender : IEmailSender
{
    private readonly EmailSenderSettings _settings;
    private readonly ILogger<PostmarkEmailSender> _logger;

    public PostmarkEmailSender(IOptions<EmailSenderSettings> options,
        ILogger<PostmarkEmailSender> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Send an email asynchronously:
        var message = new PostmarkMessage()
        {
            To = email,
            From = _settings.FromAddress,
            TrackOpens = true,
            Subject = subject,
            TextBody = htmlMessage,
            HtmlBody = htmlMessage
        };

        var client = new PostmarkClient(_settings.Key);
        _logger.LogDebug("Sending email to {Email}. Subject: {Subject}", email, subject);
        try
        {
            var sendResult = await client.SendMessageAsync(message);

            if (sendResult.Status != PostmarkStatus.Success)
            {
                _logger.LogError("Error Sending email {Email}. Status {StatusCode}. {Message}", email,
                    sendResult.ErrorCode,
                    sendResult.Message);
                throw new HttpRequestException($"Error sending email. Status {sendResult.ErrorCode}");
            }

            _logger.LogInformation("Successfully sent email to {Email}. Subject: {Subject}", email, subject);
        }
        catch (Exception e)
        {
            _logger.LogError("Unexpected Error Sending email {Email}. {Message}", email,
                e.Message);
        }
    }
}