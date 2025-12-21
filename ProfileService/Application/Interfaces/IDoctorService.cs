using Domain.Models;

namespace Application.Interfaces;

public interface IDoctorService
{
    public Task CreateDoctorAsync (string  firstName, string lastName, string? middleName,DateOnly birthDate, Guid accountId,Guid specializationId, DateOnly startWorkDate,  CancellationToken cancellationToken);
    public Task<Doctor> GetDoctorByIdAsync();
    public Task<Doctor> GetDoctorByNameAsync(string? firstName, string? lastName, string? middleName, CancellationToken cancellationToken);
    public Task<IEnumerable<Doctor>> GetDoctorsBySpecialisationAsync(string specialisation, CancellationToken cancellationToken);
    public Task<IEnumerable<Doctor>> GetDoctorsAsync (CancellationToken cancellationToken);
    public Task UpdateDoctorAsync(Doctor doctor);
    public Task DeleteDoctorAsync(Doctor doctor);
}