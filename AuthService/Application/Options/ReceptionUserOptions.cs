using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class ReceptionUserOptions
{
    public const string SectionName = "Reception";

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [MinLength(8)]
    public required string Password { get; init; }

    [Required]
    [Phone]
    public required string PhoneNumber { get; init; }
    
    [Required]
    public required string FirstName { get; init; }
    
    [Required]
    public required string LastName { get; init; }
    
    [Required]
    public string? MiddleName { get; init; }
    
    [Required]
    public DateOnly DateOfBirth { get; init; }
}