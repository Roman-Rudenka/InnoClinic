using Domain.Models;

namespace Application.Interfaces;

public interface ISpecializationService
{
    public Task CreateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken);
    public Task<Specialization> GetSpecializationByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<Specialization> GetSpecializationByNameAsync(string name, CancellationToken cancellationToken);
    public Task<IEnumerable<Specialization>> GetSpecializationsAsync(CancellationToken cancellationToken);
    public Task UpdateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken);
    public Task DeleteSpecializationAsync(Guid id, CancellationToken cancellationToken);
}