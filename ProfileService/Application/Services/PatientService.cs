using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class PatientService : IPatientService
{
    private readonly IProfileRepository<Patient> _repository;

    public PatientService(IProfileRepository<Patient> repository)
    {
        _repository = repository;
    }
    public async Task CreatePatientAsync(string  firstName, string lastName, string? middleName, DateOnly dateOfBirth, Guid accoutId, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = dateOfBirth,
            AccountId = accoutId,
            IsLinkedToAccount = true
        };
        await _repository.AddAsync(patient, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Patient> GetPatientByidAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await  _repository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ApplicationException($"Patient  not found");
        }
        return user;
    }

    public async Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(cancellationToken);
        
        return users;
    }

    public async Task UpdatePatientAsync(Guid id, string ? firstName, string? lastName, string? middleName, DateOnly? dateOfBirth, CancellationToken cancellationToken)
    {

    }

    public async Task<string> DeletePatientAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ApplicationException($"Patient not found");
        } 
        await _repository.DeleteAsync(user, cancellationToken);
        
        return "User deleted";
    }
}