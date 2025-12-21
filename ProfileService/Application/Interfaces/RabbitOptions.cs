namespace Application.Interfaces;

public class RabbitOptions
{
    public required string ConnectionUri { get; init; }
    public required string QueueName { get; init; }
}