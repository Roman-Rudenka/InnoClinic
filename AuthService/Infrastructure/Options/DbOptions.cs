using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options;

public class DbOptions
{
    public const string SectionName = "ConnectionStrings";
    [Required(AllowEmptyStrings = false)]
    public required string DefaultConnection { get; init; }
}