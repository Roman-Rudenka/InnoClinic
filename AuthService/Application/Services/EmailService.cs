using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Application.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailService(IOptions<EmailOptions> emailOptions, IOptions<RedisOptions> redisOptions, IDistributedCache cache, IUserRepository repository) : IEmailService
{
    private readonly EmailOptions _options = emailOptions.Value;
    private readonly RedisOptions _redisOptions =  redisOptions.Value;

    public async Task SendConfirmationCodeAsync(string email, string subject, string body, string code,  CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty", nameof(email));

        using var smtpClient = new SmtpClient(_options.SmtpServer, _options.Port);
        smtpClient.Credentials = new NetworkCredential(_options.Username, _options.Password);
        smtpClient.EnableSsl = true;

        var message = new MailMessage
        {
            From = new MailAddress(_options.Username, _options.FromServer),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(email);
        await smtpClient.SendMailAsync(message, cancellationToken);

        var redisKey = $"{_redisOptions.InstanceName}:email-confirm:{email}";
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        };

        await cache.SetStringAsync(redisKey, code, cacheOptions, cancellationToken);
    }

    public async Task<bool> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken)
    {
        var redisKey = $"{_redisOptions.InstanceName}:email-confirm:{email}";
        var storedCode = await cache.GetStringAsync(redisKey, cancellationToken);

        if (string.IsNullOrEmpty(storedCode) || storedCode != code)
        {
            return false;
        }

        await cache.RemoveAsync(redisKey, cancellationToken);
        await repository.UpdateEmailStatusAsync(email, cancellationToken);
        return true;
    }
}