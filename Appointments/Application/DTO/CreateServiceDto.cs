namespace Application.DTO;

public record CreateServiceDto(
    string Name, 
    decimal Price, 
    Guid CategoryId, 
    Guid SpecializationId
);