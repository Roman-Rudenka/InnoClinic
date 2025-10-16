using Domain.Common;

namespace Application.Interfaces;

public interface IProfileRepository<T>  where T : ProfileModel
{
    Task AddAsync(T profile, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(T profile, CancellationToken cancellationToken);
    Task DeleteAsync(T profile, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}