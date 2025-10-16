using Domain.Common;

namespace Application.Interfaces;

public interface IProfileRepository<T>  where T : ProfileModel
{
    Task AddAsync(T profile);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    void UpdateAsync(T profile);
    void DeleteAsync(T profile);
    Task SaveChangesAsync();
}