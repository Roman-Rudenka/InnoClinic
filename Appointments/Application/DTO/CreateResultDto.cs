namespace Application.DTO;

public record CreateResultDto(
    Guid AppointmentId, 
    string Conclusion, 
    string Recommendation
);