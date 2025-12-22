using Application.DTO;

namespace Application.Interfaces;

public interface IResultService
{
    Task<ResultDto> CreateAsync(CreateResultDto dto, CancellationToken ct);
    Task<ResultDto?> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken ct);
}