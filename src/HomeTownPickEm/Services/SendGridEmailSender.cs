using HomeTownPickEm.Config;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HomeTownPickEm.Services
{
    public class SendGridEmailSender : EmailSenderBase
    {
        public SendGridEmailSender(IOptions<SendGridSettings> options,
            IHttpContextAccessor accessor,
            IOptions<OriginOptions> originOptions, UserManager<ApplicationUser> userManager,
            ILogger<SendGridEmailSender> logger) : base(options, accessor, originOptions, userManager, logger)
        {
        }

        public override async Task SendEmailAsync(string email, string subject, string htmlMessage)
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