namespace Domain.Models;

public class Service
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid SpecializationId { get; set; }
}