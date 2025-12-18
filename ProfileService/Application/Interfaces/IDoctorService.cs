using Domain.Models;

namespace Application.Interfaces;

public interface IDoctorService
{
    public Task CreateDoctorAsync ();
    public Task<Doctor> GetDoctorByIdAsync();
    public Task<IEnumerable<Doctor>> GetDoctorsAsync (CancellationToken cancellationToken);
    public Task UpdateDoctorAsync(Doctor doctor);
    public Task DeleteDoctorAsync(Doctor doctor);
}