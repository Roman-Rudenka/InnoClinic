namespace Application.DTO;

public record ProfileDataDto(string FirstName, string LastName, string? MiddleName, DateOnly DateOfBirth, Guid AccountId);