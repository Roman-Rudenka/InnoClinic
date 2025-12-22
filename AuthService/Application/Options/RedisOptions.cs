using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class RedisOptions
{
    public const string SectionName = "Redis";
    [Required(AllowEmptyStrings = false)]
    public required string Configuration { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string InstanceName { get; init; }
}
