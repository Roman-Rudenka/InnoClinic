using Domain.Models;

namespace Application.Interfaces;

public interface IReceptionService
{ 
    Task CreateReceptionAsync(string firstName, string lastName, string? middleName, DateOnly birthDate, Guid accountId, Guid officeAddress, CancellationToken cancellationToken);
    Task<Receptionist> GetReceptionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateReceptionAsync(Guid id, string firstName, string lastName, string? middleName, DateOnly birthDate, CancellationToken cancellationToken);
    Task DeleteReceptionAsync(Guid id, CancellationToken cancellationToken);
}