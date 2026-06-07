using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Service.Services;

internal class EmailSender(IConfiguration configuration, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly string? _fromMail = configuration["Email:Address"];
    private readonly string? _fromPassword = configuration["Email:Password"];

    public Task SendEmailAsync(string email, string subject, string message)
        => Send(email, subject, message, isHtml: false);

    public Task SendHtmlEmailAsync(string email, string subject, string htmlBody)
        => Send(email, subject, htmlBody, isHtml: true);

    private async Task Send(string email, string subject, string body, bool isHtml)
    {
        // Surface configuration problems instead of failing deep inside SmtpClient.
        if (string.IsNullOrWhiteSpace(_fromMail) || string.IsNullOrWhiteSpace(_fromPassword))
        {
            logger.LogError(
                "Cannot send email to {Recipient}: 'Email:Address' or 'Email:Password' is not configured.",
                email);
            throw new InvalidOperationException("Email sender is not configured (Email:Address / Email:Password).");
        }

        logger.LogInformation("Sending email to {Recipient} with subject \"{Subject}\" (html: {IsHtml}).",
            email, subject, isHtml);

        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_fromMail, _fromPassword)
        };

        using var mail = new MailMessage
        {
            From = new MailAddress(_fromMail, "Where & How"),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };
        mail.To.Add(email);

        try
        {
            await client.SendMailAsync(mail);
            logger.LogInformation("Email successfully sent to {Recipient}.", email);
        }
        catch (SmtpException ex)
        {
            // SMTP-specific failure (auth rejected, relay denied, connection refused, etc.)
            logger.LogError(ex,
                "SMTP error sending email to {Recipient}. StatusCode: {StatusCode}. Message: {Message}",
                email, ex.StatusCode, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error sending email to {Recipient}.", email);
            throw;
        }
    }
}
