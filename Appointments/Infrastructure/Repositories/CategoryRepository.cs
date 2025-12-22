using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(AppointmentsDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await context.Categories.AddAsync(category, ct);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct)
    {
        return await context.Categories
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
}