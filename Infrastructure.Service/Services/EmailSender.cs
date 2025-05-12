using Application.Interfaces;
using System.Net.Mail;
using System.Net;


namespace Infrastructure.Service.Services;
internal class EmailSender: IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        string fromMail = "whereandhow02@gmail.com";
        string fromPassword = "ustkdafartsnnaqc";

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(fromMail, fromPassword)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromMail, "Where & How"),
            Subject = subject,
            Body = message,
            IsBodyHtml = false
        };
        mailMessage.To.Add(email);


        return client.SendMailAsync(mailMessage);
    }
}
