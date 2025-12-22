using Application.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class ProfileRepository<T>(AppDbContext context) : IProfileRepository<T>
    where T : ProfileModel
{
    public async Task AddAsync(T profile, CancellationToken cancellationToken)
    {
        await context.Set<T>().AddAsync(profile, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(T profile, CancellationToken cancellationToken)
    { 
        context.Set<T>().Update(profile);
        return Task.CompletedTask;
    }

    public Task  DeleteAsync(T profile, CancellationToken cancellationToken)
    {
        context.Set<T>().Remove(profile);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    { 
        await context.SaveChangesAsync(cancellationToken);
    }
}