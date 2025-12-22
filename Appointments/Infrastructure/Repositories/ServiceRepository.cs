using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository(AppointmentsDbContext context) : IServiceRepository
{
    public async Task AddAsync(Service service, CancellationToken ct)
    {
        await context.Services.AddAsync(service, ct);
    }

    public void Update(Service service)
    {
        context.Services.Update(service);
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<List<Service>> GetBySpecializationIdAsync(Guid specializationId, CancellationToken ct)
    {
        return await context.Services
            .AsNoTracking()
            .Include(s => s.Category)
            .Where(s => s.SpecializationId == specializationId && s.IsAvailable)
            .ToListAsync(ct);
    }

    public async Task<List<Service>> GetAllActiveAsync(CancellationToken ct)
    {
        return await context.Services
            .AsNoTracking()
            .Include(s => s.Category)
            .Where(s => s.IsAvailable)
            .ToListAsync(ct);
    }
    
    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await context.Services.AnyAsync(s => s.Id == id, ct);
    }
}