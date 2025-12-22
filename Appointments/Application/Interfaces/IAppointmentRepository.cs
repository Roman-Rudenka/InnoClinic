using Domain.Models;

namespace Application.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment, CancellationToken ct);
    void Update(Appointment appointment);
    void Remove(Appointment appointment);
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> IsSlotTakenAsync(Guid doctorId, DateOnly date, TimeOnly time, CancellationToken ct);
    Task<List<TimeOnly>> GetBusySlotsAsync(Guid doctorId, DateOnly date, CancellationToken ct);
    Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken ct);
    Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId, CancellationToken ct);
}