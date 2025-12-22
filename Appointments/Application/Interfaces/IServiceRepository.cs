using Domain.Models;

namespace Application.Interfaces;

public interface IServiceRepository
{
    Task AddAsync(Service service, CancellationToken ct);
    void Update(Service service);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Service>> GetBySpecializationIdAsync(Guid specializationId, CancellationToken ct);
    Task<List<Service>> GetAllActiveAsync(CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}