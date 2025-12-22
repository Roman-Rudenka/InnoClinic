using Domain.Models;

namespace Application.Interfaces;

public interface IDoctorService
{
    Task CreateDoctorAsync(string firstName, string lastName, string? middleName, DateOnly birthDate, Guid accountId, Guid specializationId, Guid officeId, DateOnly startWorkDate, CancellationToken ct);    
    Task<Doctor> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Doctor>> GetDoctorsBySpecialisationAsync(string specialisationName, CancellationToken cancellationToken);
    Task<IEnumerable<Doctor>> GetDoctorsAsync(CancellationToken cancellationToken);
    Task UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
    Task DeleteDoctorAsync(Guid id, CancellationToken cancellationToken);
}