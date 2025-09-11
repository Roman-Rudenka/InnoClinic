namespace Domain.Models;

/// <summary>
/// Reg
/// </summary>
public class RefreshToken
{

    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    /// <summary>
    /// Stores Token
    /// </summary>
    /// <remarks>Initialized with null due to </remarks>
    public string Token { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}