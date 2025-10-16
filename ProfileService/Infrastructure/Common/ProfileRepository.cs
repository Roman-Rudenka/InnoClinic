using Application.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class ProfileRepository<T> : IProfileRepository<T> where T : ProfileModel
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async  Task AddAsync(T profile)
    {
        await _context.AddAsync(profile);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public void UpdateAsync(T profile)
    {
         _dbSet.Update(profile);
    }

    public void DeleteAsync(T profile)
    {
        _dbSet.Remove(profile);
    }

    public async Task SaveChangesAsync()
    { 
        await _context.SaveChangesAsync();
    }
}