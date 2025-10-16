namespace Application.Interfaces;

public interface IEmailService
{
    public Task SendConfirmationCodeAsync(string email, string subject, string body, string code, CancellationToken cancellationToken);
    public Task<bool> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken);
}