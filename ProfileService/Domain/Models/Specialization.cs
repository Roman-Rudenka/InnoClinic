namespace Domain.Models;

public class Specialization
{
    public Guid Id { get; set; } =  Guid.CreateVersion7();
    public required string SpecializationName { get; set; }
    public bool IsActive { get; set; }
}