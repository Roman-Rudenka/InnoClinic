namespace Application.DTO;


public class ProfileDataDto
{
    public Guid AccountId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    public string? OfficeAddress { get; set; } 
    public string? SpecializationName { get; set; }
    public DateOnly? StartWorkDate { get; set; }
}