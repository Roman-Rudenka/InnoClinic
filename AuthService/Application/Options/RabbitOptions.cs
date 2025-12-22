using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class RabbitOptions
{
    public const string SectionName = "Rabbit";
    [Required(AllowEmptyStrings = false)]
    public required string ConnectionUri { get; init; }
    
    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public required string QueueName { get; init; }
}