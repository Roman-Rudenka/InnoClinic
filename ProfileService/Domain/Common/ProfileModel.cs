namespace Domain.Common;

public class ProfileModel
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool IsLinkedToAccount { get; set; }
    public DateOnly BirthDate { get; set; }
}