using Application.DTO;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class ServicesService(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork) : IServicesService
{
    public async Task CreateServiceAsync(CreateServiceDto dto, CancellationToken ct)
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            SpecializationId = dto.SpecializationId,
            IsAvailable = true
        };

        await serviceRepository.AddAsync(service, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<List<ServiceDto>> GetBySpecializationAsync(Guid specializationId, CancellationToken ct)
    {
        var services = await serviceRepository.GetBySpecializationIdAsync(specializationId, ct);
        return services.Select(s => new ServiceDto(s.Id, s.Name, s.Price, s.Category?.Name ?? "Unknown", s.SpecializationId)).ToList();
    }

    public async Task<List<ServiceDto>> GetAllActiveAsync(CancellationToken ct)
    {
        var services = await serviceRepository.GetAllActiveAsync(ct);
        return services.Select(s => new ServiceDto(s.Id, s.Name, s.Price, s.Category?.Name ?? "Unknown", s.SpecializationId)).ToList();
    }
}