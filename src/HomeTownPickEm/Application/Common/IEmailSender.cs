namespace HomeTownPickEm.Application.Common;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}