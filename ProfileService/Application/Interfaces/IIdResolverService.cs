namespace Application.Interfaces;

public interface IIdResolverService
{
    Task<Guid> ResolveOfficeIdAsync(string address, CancellationToken ct);
    Task<Guid> ResolveSpecializationIdAsync(string name, CancellationToken ct); 
}