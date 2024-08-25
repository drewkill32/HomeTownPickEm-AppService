using System.Text;
using System.Text.Json;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Config;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Services;

public class ResendEmailSender : IEmailSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ResendEmailSender> _logger;
    private readonly EmailSenderSettings _settings;

    public ResendEmailSender(HttpClient httpClient, IOptions<EmailSenderSettings> options,
        ILogger<ResendEmailSender> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (subject != null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new("https://api.resend.com/emails"),
                Headers =
                {
                    { "Authorization", $"Bearer {_settings.Key}" }
                },
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    from = _settings.FromAddress,
                    to = new[] { email },
                    subject = subject,
                    html = htmlMessage
                }), Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                _logger.LogDebug("Sending email to {ToEmail}", email);
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var emailResponse = JsonSerializer.Deserialize<EmailResponse>(responseBody, options);
                _logger.LogInformation("Successfully sent email to {ToEmail}, Id: {Id}", email, emailResponse.Id);
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to send email to {ToEmail}. {Message}", email, e.Message);
            }
        }
    }

    private class EmailResponse
    {
        public string Id { get; set; }
    }
}