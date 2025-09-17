using Domain.Models;

namespace Application.Interfaces;

public interface ITokenRepository
{
    public Task CreateRefreshTokenAsync(RefreshToken token,  CancellationToken cancellationToken);
    
    public Task<RefreshToken?> GetRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken);
    public Task DeleteRefreshTokenAsync(string token, CancellationToken cancellationToken);
    public Task DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}