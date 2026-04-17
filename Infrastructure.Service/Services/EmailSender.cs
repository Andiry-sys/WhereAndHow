using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Service.Services;

internal class EmailSender(IConfiguration configuration) : IEmailSender
{
    private readonly string _fromMail = configuration["Email:Address"];
    private readonly string _fromPassword = configuration["Email:Password"];

    public Task SendEmailAsync(string email, string subject, string message)
        => Send(email, subject, message, isHtml: false);

    public Task SendHtmlEmailAsync(string email, string subject, string htmlBody)
        => Send(email, subject, htmlBody, isHtml: true);

    private Task Send(string email, string subject, string body, bool isHtml)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_fromMail, _fromPassword)
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_fromMail, "Where & How"),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };
        mail.To.Add(email);

        return client.SendMailAsync(mail);
    }
}
