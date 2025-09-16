namespace Application.AuthDTO;

public record RefreshTokensDto
{ 
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}