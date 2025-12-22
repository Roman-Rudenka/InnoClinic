using Application.DTO;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IServiceRepository serviceRepository,
    IResultRepository resultRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IAppointmentService
{
    public async Task<List<TimeSlotDto>> GetFreeSlotsAsync(Guid doctorId, DateOnly date, CancellationToken ct)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return [];

        var busyTimes = await appointmentRepository.GetBusySlotsAsync(doctorId, date, ct);
        var slots = new List<TimeSlotDto>();
        var currentTime = new TimeOnly(9, 0);
        var endTime = new TimeOnly(18, 0);
        var step = TimeSpan.FromMinutes(15);
        
        var nowUtc = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(nowUtc);
        var timeNow = TimeOnly.FromDateTime(nowUtc);

        while (currentTime < endTime)
        {
            bool isFree = !busyTimes.Contains(currentTime);
            if (date < today) isFree = false;
            if (date == today && currentTime < timeNow) isFree = false;

            slots.Add(new TimeSlotDto(currentTime, isFree));
            currentTime = currentTime.Add(step);
        }
        return slots;
    }

    public async Task CreateAppointmentAsync(CreateAppointmentDto dto, CancellationToken ct)
    {
        var patientId = userContext.GetUserId();

        if (dto.Time.Hour < 9 || dto.Time.Hour >= 18)
            throw new InvalidOperationException("Doctor works from 09:00 to 18:00.");

        var service = await serviceRepository.GetByIdAsync(dto.ServiceId, ct);
        if (service == null || !service.IsAvailable)
            throw new KeyNotFoundException("Service not found or unavailable.");

        if (await appointmentRepository.IsSlotTakenAsync(dto.DoctorId, dto.Date, dto.Time, ct))
            throw new InvalidOperationException("Slot is already taken.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = dto.DoctorId,
            PatientId = patientId,
            ServiceId = dto.ServiceId,
            Date = dto.Date,
            Time = dto.Time,
            IsApproved = false
        };

        await appointmentRepository.AddAsync(appointment, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
    
    public async Task AddResultAsync(CreateResultDto dto, CancellationToken ct)
    {
        var doctorId = userContext.GetUserId();
        var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId, ct);

        if (appointment == null) throw new KeyNotFoundException("Appointment not found.");
        if (appointment.DoctorId != doctorId) throw new UnauthorizedAccessException("Not your appointment.");
        if (appointment.Result != null) throw new InvalidOperationException("Result already exists.");

        var result = new Result
        {
            Id = Guid.NewGuid(),
            AppointmentId = dto.AppointmentId,
            Conclusion = dto.Conclusion,
            Recommendation = dto.Recommendation
        };

        await resultRepository.AddAsync(result, ct);
        
        if (!appointment.IsApproved)
        {
            appointment.IsApproved = true;
            appointmentRepository.Update(appointment);
        }
        
        await unitOfWork.SaveChangesAsync(ct);
    }
    
    public async Task ChangeStatusAsync(Guid appointmentId, bool isApproved, CancellationToken ct)
    {
        var doctorId = userContext.GetUserId();
        var appointment = await appointmentRepository.GetByIdAsync(appointmentId, ct);

        if (appointment == null) throw new KeyNotFoundException("Appointment not found");
        if (appointment.DoctorId != doctorId) throw new UnauthorizedAccessException("Only the doctor can approve appointments.");

        appointment.IsApproved = isApproved;
        appointmentRepository.Update(appointment);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<List<AppointmentDto>> GetMyAppointmentsAsync(CancellationToken ct)
    {
        var userId = userContext.GetUserId();
        var role = userContext.GetUserRole();

        List<Appointment> appointments;
        if (role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
            appointments = await appointmentRepository.GetByDoctorIdAsync(userId, ct);
        else
            appointments = await appointmentRepository.GetByPatientIdAsync(userId, ct);

        return appointments.Select(a => new AppointmentDto(
            a.Id, a.DoctorId, a.PatientId, a.Date, a.Time, a.IsApproved,
            a.Service != null ? new ServiceDto(a.Service.Id, a.Service.Name, a.Service.Price, a.Service.Category?.Name ?? "Unknown", a.Service.SpecializationId) : null,
            a.Result != null ? new ResultDto(a.Result.Id, a.Result.AppointmentId, a.Result.Conclusion, a.Result.Recommendation) : null
        )).ToList();
    }
}