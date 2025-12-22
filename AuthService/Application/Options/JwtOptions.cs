using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required(AllowEmptyStrings = false)]
    public required string Issuer { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Audience { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Secret { get; init; }

    [Range(1, int.MaxValue)]
    public required int AccessTokenLifetimeMinutes { get; init; }

    [Range(1, int.MaxValue)]
    public required int RefreshTokenLifetimeDays { get; init; }
}
