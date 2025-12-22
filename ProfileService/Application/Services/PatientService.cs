using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class PatientService(IPatientRepository repository) : IPatientService
{
    public async Task CreatePatientAsync(string firstName, string lastName, string? middleName, DateOnly birthDate, Guid accountId, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = birthDate,
            AccountId = accountId,
            IsLinkedToAccount = true
        };

        await repository.AddAsync(patient, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Patient> GetPatientByidAsync(Guid id, CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        if (patient == null) throw new KeyNotFoundException("Patient not found.");
        
        return patient;
    }

    public async Task<Patient> GetByNameAsync(string? firstName, string? lastName, string? middleName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            throw new ArgumentException("First and Last name are required for search.");

        var patient = await repository.GetByNameAsync(firstName, lastName, cancellationToken);
        
        if (patient == null) throw new KeyNotFoundException("Patient not found.");
        
        return patient;
    }

    public async Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task UpdatePatientAsync(Guid id, string? firstName, string? lastName, string? middleName, DateOnly? dateOfBirth, CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        if (patient == null) throw new KeyNotFoundException("Patient not found.");

        if (!string.IsNullOrEmpty(firstName)) patient.FirstName = firstName;
        if (!string.IsNullOrEmpty(lastName)) patient.LastName = lastName;
        if (!string.IsNullOrEmpty(middleName)) patient.MiddleName = middleName;
        if (dateOfBirth.HasValue) patient.BirthDate = dateOfBirth.Value;

        await repository.UpdateAsync(patient, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> DeletePatientAsync(Guid id, CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        if (patient == null) throw new KeyNotFoundException("Patient not found.");

        await repository.DeleteAsync(patient, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        
        return "Patient deleted successfully.";
    }
}