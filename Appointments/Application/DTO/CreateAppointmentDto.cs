namespace Application.DTO;

public record CreateAppointmentDto(
    Guid DoctorId, 
    Guid ServiceId, 
    DateOnly Date, 
    TimeOnly Time
);