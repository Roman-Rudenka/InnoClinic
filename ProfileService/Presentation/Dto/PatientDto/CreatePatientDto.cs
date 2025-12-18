namespace Presentation.Dto.PatientDto;

public record CreatePatientDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
    public DateOnly DateOfBirth { get; init; }
}