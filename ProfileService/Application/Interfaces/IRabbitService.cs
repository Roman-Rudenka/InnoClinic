namespace Application.Interfaces;

public interface IRabbitService
{
    public Task<Guid> GetIdFromRabbit();
}