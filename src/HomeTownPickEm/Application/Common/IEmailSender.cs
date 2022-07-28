using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Common;

public enum EmailType
{
    NewUser,
    Register,
    ForgotPassword
}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendEmailAsync(EmailType emailType, ApplicationUser user);
    
    
}