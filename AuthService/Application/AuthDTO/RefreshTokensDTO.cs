namespace Application.AuthDTO;

public record RefreshTokensDTO
{ 
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}