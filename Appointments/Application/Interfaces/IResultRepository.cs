using Domain.Models;

namespace Application.Interfaces;

public interface IResultRepository
{
    Task AddAsync(Result result, CancellationToken ct);
    Task<Result?> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken ct);
}