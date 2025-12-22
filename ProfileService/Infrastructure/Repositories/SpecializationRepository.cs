using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SpecializationRepository(AppDbContext context) : ISpecializationRepository
{
    public async Task AddAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        await context.Specializations.AddAsync(specialization, cancellationToken);
    }

    public async Task<Specialization?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Specializations.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Specializations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        context.Specializations.Update(specialization);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        context.Specializations.Remove(specialization);
        return Task.CompletedTask;
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Specialization?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Specializations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SpecializationName == name, cancellationToken);
    }
}