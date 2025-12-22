namespace Application.DTO;

public record AppointmentDto(
    Guid Id,
    Guid DoctorId,
    Guid PatientId,
    DateOnly Date,
    TimeOnly Time,
    bool IsApproved,
    ServiceDto? Service,
    ResultDto? Result
);