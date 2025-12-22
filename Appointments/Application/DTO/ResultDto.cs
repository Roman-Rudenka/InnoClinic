namespace Application.DTO;

public record ResultDto(
    Guid Id, 
    Guid AppointmentId,
    string Conclusion, 
    string Recommendation
);