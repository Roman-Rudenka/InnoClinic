namespace Application.DTO;

public record ServiceDto(
    Guid Id, 
    string Name, 
    decimal Price, 
    string CategoryName,
    Guid SpecializationId
);