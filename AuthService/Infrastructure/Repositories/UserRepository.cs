using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FindAsync(id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> UpdateEmailStatusAsync(string email, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user is null)
        {
            throw new NullReferenceException("User not found");
        }
        user.EmailConfirmed = true;
        return user;
    }
}