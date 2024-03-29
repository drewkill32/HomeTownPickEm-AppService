﻿using System.Net.Mail;
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
        var smtpClient = new SmtpClient()
        {
            Host = "localhost",
            Port = 25
        };
        try
        {
            var message = new MailMessage()
            {
                Body = htmlMessage,
                To = { email },
                From = new MailAddress("st-pete-pickem@localhost"),
                Subject = subject,
                IsBodyHtml = true
            };
            smtpClient.Send(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send email. Enable smtp4dev and try again. {Message}", e.Message);
            var file = $"{Path.GetTempFileName()}.html";
            File.WriteAllText(file, htmlMessage);
            _logger.LogInformation("Sending email to: {Email}. {Subject}, {Path}", email, subject, file);
            _logger.LogInformation("To: {Email}. {Message}", email, htmlMessage);
        }
        finally
        {
            smtpClient.Dispose();
        }


        return Task.CompletedTask;
    }
}