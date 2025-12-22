using Domain.Models;

namespace Application.Interfaces;

public interface IPatientRepository : IProfileRepository<Patient>
{
    Task<Patient?> GetByNameAsync(string firstName, string lastName, CancellationToken cancellationToken);
}