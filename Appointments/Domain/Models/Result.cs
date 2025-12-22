namespace Domain.Models;

public class Result
{
    public Guid Id { get; set; }
    public required string Conclusion { get; set; }
    public required string Recommendation { get; set; }
    
    public Guid AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
}