using Application.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class ProfileRepository<T>(AppDbContext context, DbSet<T> dbSet) : IProfileRepository<T>
    where T : ProfileModel
{
    public async  Task AddAsync(T profile,  CancellationToken cancellationToken)
    {
        await context.AddAsync(profile,  cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id , CancellationToken cancellationToken)
    {
        return await context.FindAsync<T>(id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(T profile, CancellationToken cancellationToken)
    { 
        dbSet.Update(profile);
        await context.SaveChangesAsync(cancellationToken);
        
    }

    public async Task  DeleteAsync(T profile, CancellationToken cancellationToken)
    {
        dbSet.Remove(profile);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    { 
        await context.SaveChangesAsync(cancellationToken);
    }
}