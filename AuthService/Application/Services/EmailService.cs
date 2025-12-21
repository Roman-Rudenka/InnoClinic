using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Application.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailService(IOptions<EmailOptions> emailOptions) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Email cannot be null or empty", nameof(toEmail));
        
        using var smtpClient = new SmtpClient(emailOptions.Value.SmtpServer, emailOptions.Value.Port)
        {
            Credentials = new NetworkCredential(emailOptions.Value.Username, emailOptions.Value.Password),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(emailOptions.Value.Username, emailOptions.Value.FromServer),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);
        
        await smtpClient.SendMailAsync(message, cancellationToken);
    }
}