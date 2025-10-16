using Domain.Models;

namespace Application.Interfaces;

public interface IPatientService
{
    public Task CreatePatientAsync(string firstName, string lastName, string? middleName, DateOnly birthDate);
    public Task<Patient> GetPatientByidAsync(Guid id);
    public Task<IEnumerable<Patient>> GetPatientsAsync();
    public Task UpdatePatientAsync(Guid id,  string firstName, string lastName, string? middleName, DateOnly? dateOfBirth);
    public Task DeletePatientAsync();
}