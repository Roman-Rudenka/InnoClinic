namespace Presentation.Requests;

public record ConfirmEmail
{
    public required string Email { get; init; }
}