namespace Domain.Models;

/// <summary>
/// Model for refresh token
/// </summary>
public class RefreshToken
{

    public Guid Id { get; init; } = Guid.CreateVersion7();
    
    /// <summary>
    /// Stores Token
    /// </summary>
    /// <remarks>Initialized with null to avoid troubles with refreshing </remarks>
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}