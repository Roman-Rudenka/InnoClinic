using Domain.Models;

namespace Application.Interfaces;

public interface ISpecializationRepository
{ 
        Task AddAsync(Specialization specialization, CancellationToken cancellationToken);
        Task<Specialization?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Specialization specialization, CancellationToken cancellationToken); 
        Task DeleteAsync(Specialization specialization, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<Specialization?> GetByNameAsync(string name, CancellationToken cancellationToken);
}