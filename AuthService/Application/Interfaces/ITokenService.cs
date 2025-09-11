using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// generation access token for user 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>access token</returns>
    public string GenerateAccessToken(Guid userId, string email, CancellationToken cancellationToken);
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
    /// <param name="cancellationToken"></param>
    /// <returns>is token is valid (true/false)</returns>
    public ClaimsPrincipal? ValidateAccessToken(string token, CancellationToken cancellationToken);
    /// <summary>
    /// checking refresh token if it us valid
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
    /// <param name="cancellationToken"></param>
    /// <returns>new token</returns>
    public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);
}