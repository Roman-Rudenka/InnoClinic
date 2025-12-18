using Application.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class ProfileRepository<T> : IProfileRepository<T> where T : ProfileModel
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async  Task AddAsync(T profile,  CancellationToken cancellationToken)
    {
        await _context.AddAsync(profile,  cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id , CancellationToken cancellationToken)
    {
        return await _context.FindAsync<T>(id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(T profile, CancellationToken cancellationToken)
    { 
        _dbSet.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
        
    }

    public async Task  DeleteAsync(T profile, CancellationToken cancellationToken)
    {
        _dbSet.Remove(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    { 
        await _context.SaveChangesAsync(cancellationToken);
    }
}