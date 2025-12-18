using Domain.Models;

namespace Application.Interfaces;

public interface IPatientService
{
    public Task CreatePatientAsync(string firstName, string lastName, string? middleName, DateOnly birthDate, Guid accoutId, CancellationToken cancellationToken);
    public Task<Patient> GetPatientByidAsync(Guid id, CancellationToken cancellationToken);
    public Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken);
    public Task UpdatePatientAsync(Guid id,  string? firstName, string? lastName, string? middleName, DateOnly? dateOfBirth, CancellationToken cancellationToken);
    public Task<string> DeletePatientAsync(Guid id, CancellationToken cancellationToken);
}