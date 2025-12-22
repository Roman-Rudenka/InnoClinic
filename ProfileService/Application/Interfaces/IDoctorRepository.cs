using Domain.Models;

namespace Application.Interfaces;

public interface IDoctorRepository : IProfileRepository<Doctor>
{
    Task<IEnumerable<Doctor>> GetBySpecializationIdAsync(Guid specializationId, CancellationToken cancellationToken);
}