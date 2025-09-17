using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TokenRepository(AppDbContext context) : ITokenRepository
{
    public async Task CreateRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.UserId == userId, cancellationToken: cancellationToken);
    }

    public async Task DeleteRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var existing = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken: cancellationToken);
        if (existing != null)
        {
            context.RefreshTokens.Remove(existing);
        }
    }

    public async Task DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var existing = await  context.RefreshTokens.FirstOrDefaultAsync(rt =>rt.UserId == userId, cancellationToken: cancellationToken);
        if (existing != null)
        {
            context.RefreshTokens.Remove(existing);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}