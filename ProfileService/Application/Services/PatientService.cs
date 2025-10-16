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
    public async Task CreatePatientAsync(string  firstName, string lastName, string? middleName, DateOnly dateOfBirth)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = dateOfBirth,
            IsLinkedToAccount = false
        };
        await _repository.AddAsync(patient);
        await _repository.SaveChangesAsync();
    }

    public Task<Patient> GetPatientByidAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task UpdatePatientAsync(Guid id, string ? firstName, string? lastName, string? middleName, DateOnly? dateOfBirth)
    {
        if (id == Guid.Empty)
        {
            throw new Exception("ID cannot be empty");
        }
        if(firstName == null)
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = dateOfBirth,
            IsLinkedToAccount = false
        }
        await _repository.UpdateAsync(id, firstName, lastName, middleName, dateOfBirth);
    }

    public Task DeletePatientAsync()
    {
        throw new NotImplementedException();
    }
}