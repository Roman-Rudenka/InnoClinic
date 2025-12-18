namespace Presentation.Requests;

public record RegisterRequest(string Email, string Password, string PhoneNumber, string FirstName, string LastName, string? MiddleName, DateOnly DateOfBirth);
