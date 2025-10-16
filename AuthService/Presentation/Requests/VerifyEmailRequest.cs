namespace Presentation.Requests;

public record VerifyEmailRequest
{
    public required string Code { get; set; }
}