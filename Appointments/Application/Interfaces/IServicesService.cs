using Application.DTO;
using Domain.Models;

namespace Application.Interfaces;

public interface IServicesService
{
    Task CreateServiceAsync(CreateServiceDto dto, CancellationToken ct);
    Task<List<ServiceDto>> GetBySpecializationAsync(Guid specializationId, CancellationToken ct);
    Task<List<ServiceDto>> GetAllActiveAsync(CancellationToken ct);
}