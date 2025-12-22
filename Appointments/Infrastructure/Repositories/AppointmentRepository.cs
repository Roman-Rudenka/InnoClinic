using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class AppointmentRepository(AppointmentsDbContext context) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment, CancellationToken ct)
    {
        await context.Appointments.AddAsync(appointment, ct);
    }

    public void Update(Appointment appointment)
    {
        context.Appointments.Update(appointment);
    }

    public void Remove(Appointment appointment)
    {
        context.Appointments.Remove(appointment);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Appointments
            .Include(a => a.Service)
            .Include(a => a.Result)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<bool> IsSlotTakenAsync(Guid doctorId, DateOnly date, TimeOnly time, CancellationToken ct)
    {
        return await context.Appointments
            .AnyAsync(a => a.DoctorId == doctorId && a.Date == date && a.Time == time, ct);
    }

    public async Task<List<TimeOnly>> GetBusySlotsAsync(Guid doctorId, DateOnly date, CancellationToken ct)
    {
        return await context.Appointments
            .Where(a => a.DoctorId == doctorId && a.Date == date)
            .Select(a => a.Time)
            .ToListAsync(ct);
    }

    public async Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken ct)
    {
        return await context.Appointments
            .AsNoTracking()
            .Include(a => a.Service)
            .Include(a => a.Result)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.Date)
            .ThenBy(a => a.Time)
            .ToListAsync(ct);
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId, CancellationToken ct)
    {
        return await context.Appointments
            .AsNoTracking()
            .Include(a => a.Service)
            .Include(a => a.Result)
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.Date)
            .ThenBy(a => a.Time)
            .ToListAsync(ct);
    }
}