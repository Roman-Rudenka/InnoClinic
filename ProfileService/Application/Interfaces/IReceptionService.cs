using Domain.Models;

namespace Application.Interfaces;

public interface IReceptionService
{
    public Task CreateReceptionAsync(string firstName, string lastName, string? middleName, DateOnly birthDate,
        Guid accountId, CancellationToken cancellationToken);
    public Task<Receptionist>  GetReceptionByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<Receptionist> UpdateReceptionAsync(string firstName, string lastName, string? middleName, DateOnly birthDate,
        Guid accountId, CancellationToken cancellationToken);
    public Task DeleteReceptionAsync(Guid id, CancellationToken cancellationToken);
}