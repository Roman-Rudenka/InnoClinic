using Application.DTO;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class CategoryService(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<CategoryDto> CreateAsync(string name, CancellationToken ct)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await categoryRepository.AddAsync(category, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new CategoryDto(category.Id, category.Name);
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        var categories = await categoryRepository.GetAllAsync(ct);
        return categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
    }
}