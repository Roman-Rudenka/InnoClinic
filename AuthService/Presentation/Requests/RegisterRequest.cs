namespace Presentation.Requests;

public record RegisterRequest(string Email, string Password, string PhoneNumber)
{
    public required string Email { get; init; } = Email;
    public required string Password { get; init; } = Password;
    public required string PhoneNumber { get; init; } = PhoneNumber;
}