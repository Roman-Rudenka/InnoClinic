using Application.DTO;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(string name, CancellationToken ct);
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);
}