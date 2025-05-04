using Application.Interfaces;
using System.Net.Mail;
using System.Net;


namespace Infrastructure.Service.Services;
internal class EmailSender: IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        string fromMail = "whereandhow01@gmail.com";
        string fromPassword = "dtvhcdkqrvxtojoc";

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(fromMail, fromPassword)
        };

        return client.SendMailAsync(new MailMessage(fromMail, email, subject, message));
    }
}
