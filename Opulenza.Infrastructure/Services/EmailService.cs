using ErrorOr;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Application.Settings;
using Opulenza.Infrastructure.Settings;

namespace Opulenza.Infrastructure.Services;

public class EmailService(IOptions<SmtpSettings> smtpSettingsOptions, IValidator<Email> validator)
    : IEmailService
{
    private SmtpSettings SmtpSettings => smtpSettingsOptions.Value;

    public async Task<ErrorOr<string>> SendEmailAsync(Email emailRequest)
    {
        var validationResult = await validator.ValidateAsync(emailRequest);
        if (validationResult.IsValid == false)
        {
            return validationResult.Errors.Select(error =>
                Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
        }

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(SmtpSettings.SenderName, SmtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(emailRequest.To, emailRequest.To));
        email.Subject = emailRequest.Subject;

        var bodyBuilder = new BodyBuilder();

        if (emailRequest.IsBodyHtml)
        {
            bodyBuilder.HtmlBody = emailRequest.Body;
        }
        else
        {
            bodyBuilder.TextBody = emailRequest.Body;
        }

        email.Body = bodyBuilder.ToMessageBody();

        using (var smtp = new SmtpClient())
        {
            try
            {
                await smtp.ConnectAsync(SmtpSettings.Server, SmtpSettings.Port,
                    SmtpSettings.UseSsl
                        ? MailKit.Security.SecureSocketOptions.StartTls
                        : MailKit.Security.SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(SmtpSettings.Username, SmtpSettings.Password);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        return "Email sent successfully";
    }
}