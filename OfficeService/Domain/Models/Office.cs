namespace Domain.Models;

public class Office
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Address { get; set; }
    public string? RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
}