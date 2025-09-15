namespace Application.Interfaces;

public interface IRoleSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
