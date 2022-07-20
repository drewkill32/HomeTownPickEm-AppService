using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Config;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HomeTownPickEm.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ILogger<SendGridEmailSender> _logger;
        private readonly SendGridSettings _settings;

        public SendGridEmailSender(IOptions<SendGridSettings> options, ILogger<SendGridEmailSender> logger)
        {
            _logger = logger;
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            var client = new SendGridClient(_settings.Key);
            var from = new EmailAddress(_settings.Email);
            var to = new EmailAddress(email);
            var plainTextContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            _logger.LogDebug("Sending email to {Email}. Subject: {Subject}", email, subject);
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error Sending email {Email}. Status {StatusCode}", email, response.StatusCode);
                throw new HttpRequestException($"Error sending email. Status {response.StatusCode}");
            }
            
            _logger.LogInformation("Successfully sent email to {Email}. Subject: {Subject}", email, subject);
        }
    }
}