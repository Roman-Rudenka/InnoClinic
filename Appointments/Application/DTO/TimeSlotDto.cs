namespace Application.DTO;

public record TimeSlotDto(
    TimeOnly Time, 
    bool IsFree
);