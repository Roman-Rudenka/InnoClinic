namespace Application.AuthDTO;

public record ProfileDataRabbit(string FirstName, string LastName, string? MiddleName, DateOnly DateOfBirth, Guid AccountId, string? OfficeAddress = null, string? SpecializationName = null, DateOnly? StartWorkDate = null )
    : ProfileDataDto(FirstName, LastName, MiddleName, DateOfBirth, AccountId);