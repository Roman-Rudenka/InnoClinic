using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TokenRepository(AppDbContext context) : ITokenRepository
{
    public async Task CreateRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token && rt.UserId == userId, cancellationToken);
    }

    public Task DeleteRefreshTokenAsync(RefreshToken token)
    { 
        context.RefreshTokens.Remove(token);
        
        return Task.CompletedTask;
    }

    public async Task DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.Where(rt => rt.UserId == userId).ExecuteDeleteAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}