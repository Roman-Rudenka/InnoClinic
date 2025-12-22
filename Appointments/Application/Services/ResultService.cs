using Application.DTO;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class ResultService(
    IResultRepository resultRepository,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IResultService
{
    public async Task<ResultDto> CreateAsync(CreateResultDto dto, CancellationToken ct)
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
        return new ResultDto(result.Id, result.AppointmentId, result.Conclusion, result.Recommendation);
    }

    public async Task<ResultDto?> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken ct)
    {
        var userId = userContext.GetUserId();
        var appointment = await appointmentRepository.GetByIdAsync(appointmentId, ct);
        
        if (appointment == null) return null;
        if (appointment.PatientId != userId && appointment.DoctorId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        if (appointment.Result == null) return null;

        return new ResultDto(appointment.Result.Id, appointment.Result.AppointmentId, appointment.Result.Conclusion, appointment.Result.Recommendation);
    }
}