using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class PatientService : IPatientService
{
    private readonly IProfileRepository<Patient> _repository;

    PatientService(IProfileRepository<Patient> repository)
    {
        _repository = repository;
    }
    public async Task CreatePatientAsync(string  firstName, string lastName, string? middleName, DateOnly dateOfBirth, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = dateOfBirth,
            IsLinkedToAccount = false
        };
        await _repository.AddAsync(patient, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Patient> GetPatientByidAsync(Guid id)
    {
        var user = await  _repository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ApplicationException($"Patient  not found");
        }
        return user;
    }

    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        var users = await _repository.GetAllAsync();
        
        return users;
    }

    public async Task UpdatePatientAsync(Guid id, string ? firstName, string? lastName, string? middleName, DateOnly? dateOfBirth)
    {

    }

    public async Task<string> DeletePatientAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ApplicationException($"Patient not found");
        } 
        _repository.DeleteAsync(user);
        
        return "User deleted";
    }
}