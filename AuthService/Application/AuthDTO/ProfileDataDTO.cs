namespace Application.AuthDTO;

public record ProfileDataDto(string FirstName, string LastName, string? MiddleName, DateOnly DateOfBirth, Guid AccountId);