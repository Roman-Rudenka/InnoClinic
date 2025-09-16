namespace Presentation.Requests;

public record LoginRequest(string Email, string Password)
{
    public required string Email { get; init; } = Email;
    public required string Password { get; init; } = Password;
}