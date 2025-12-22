using Domain.Models;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken ct);
    Task<List<Category>> GetAllAsync(CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
}