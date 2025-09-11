namespace Application.AuthAdditions;

public record AuthResult
{ 
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}