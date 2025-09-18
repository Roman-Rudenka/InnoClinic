namespace Domain.Models;

/// <summary>
/// Model for refresh token
/// </summary>
public class RefreshToken
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}