namespace Application.Options;

public class RedisOptions
{
    public required string Configuration { get; init; }
    public required string InstanceName { get; init; }
}
