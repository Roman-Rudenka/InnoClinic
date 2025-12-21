using System.ComponentModel.DataAnnotations;

namespace Application.Options;

public class EmailOptions
{
    public const string SectionName = "EmailSettings";

    [Required(AllowEmptyStrings = false)]
    public required string SmtpServer { get; init; }

    [Range(1, 1000)] public required int Port { get; init; }

    [Required(AllowEmptyStrings = false)] 
    public required string Username { get; init; } 

    [Required(AllowEmptyStrings = false)]
    public required string Password { get; init; }
    
    public required string FromServer { get; init; }
}