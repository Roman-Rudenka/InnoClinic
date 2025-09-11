namespace Application.Options;

public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
    public required int AccessTokenLifetimeMinutes { get; init; }
    public required int RefreshTokenLifetimeDays { get; init; }
}
