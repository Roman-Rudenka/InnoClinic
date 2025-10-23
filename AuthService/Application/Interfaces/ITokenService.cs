using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// generation access token for user 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    /// <param name="roles"></param>
    /// <returns>access token</returns>
    public string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles);
    /// <summary>
    /// generating refresh token for user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>returns refresh token</returns>
    public Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken);
    /// <summary>
    /// checking access token if it is valid
    /// </summary>
    /// <param name="token"></param>
    /// <returns> valid token? (true/false)</returns>
    public ClaimsPrincipal? GetPrincipalFromAccessToken(string token);
    /// <summary>
    /// Getting userIdFromExpired Access Token to get a new pair
    /// </summary>
    /// <param name="token"></param>
    /// <returns>user id</returns>
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    /// <summary>
    /// checking refresh token if it is valid
    /// </summary>
    /// <param name="token"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>is refresh token is valid (true/false)</returns>
    public Task<bool> ValidateRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken);
    /// <summary>
    /// removing old refresh token from db and adding new
    /// </summary>
    /// <param name="token"></param>
    /// <param name="userId"></param>>
    /// <param name="cancellationToken"></param>
    /// <returns>new token</returns>
    public Task RevokeRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken);
    /// <summary>
    /// removing tokens after logout
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> is removed (true/false)</returns>
    public Task RevokeRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}