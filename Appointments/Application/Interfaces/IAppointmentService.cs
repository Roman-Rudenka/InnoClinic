using Application.DTO;
using Domain.Models;

namespace Application.Interfaces;

public interface IAppointmentService
{
    Task<List<TimeSlotDto>> GetFreeSlotsAsync(Guid doctorId, DateOnly date, CancellationToken ct);
    Task CreateAppointmentAsync(CreateAppointmentDto dto, CancellationToken ct);
    Task AddResultAsync(CreateResultDto dto, CancellationToken ct);
    Task ChangeStatusAsync(Guid appointmentId, bool isApproved, CancellationToken ct);
    Task<List<AppointmentDto>> GetMyAppointmentsAsync(CancellationToken ct);
}