using Application.Interfaces;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

public class DoctorService(IDoctorRepository doctorRepository, ISpecializationRepository specializationRepository) : IDoctorService
{
    public async Task CreateDoctorAsync(string firstName, string lastName, string? middleName, DateOnly birthDate, 
        Guid accountId, Guid specializationId, Guid officeId, DateOnly startWorkDate, CancellationToken ct)
    {
        var doctor = new Doctor
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = birthDate,
            AccountId = accountId,
            IsLinkedToAccount = true,
            SpecializationId = specializationId,
            OfficeId = officeId,
            StartWorkingDate = startWorkDate,
            Status = DoctorStatus.AtWork
        };

        await doctorRepository.AddAsync(doctor, ct);
        await doctorRepository.SaveChangesAsync(ct);
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdAsync(id, cancellationToken);
        if (doctor == null) throw new KeyNotFoundException("Doctor not found.");
        return doctor;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialisationAsync(string specialisationName, CancellationToken cancellationToken)
    {
        var spec = await specializationRepository.GetByNameAsync(specialisationName, cancellationToken);
        if (spec == null)
        {
            throw new KeyNotFoundException("Specialization not found.");
        }
        
        return await doctorRepository.GetBySpecializationIdAsync(spec.Id, cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync(CancellationToken cancellationToken)
    {
        return await doctorRepository.GetAllAsync(cancellationToken);
    }

    public async Task UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken)
    {
        await doctorRepository.UpdateAsync(doctor, cancellationToken);
        await doctorRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDoctorAsync(Guid id, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdAsync(id, cancellationToken);
        if (doctor == null) throw new KeyNotFoundException("Doctor not found.");

        await doctorRepository.DeleteAsync(doctor, cancellationToken);
        await doctorRepository.SaveChangesAsync(cancellationToken);
    }
}