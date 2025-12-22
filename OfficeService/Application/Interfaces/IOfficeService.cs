using Application.DTOs;
using Domain.Models;

namespace Application.Interfaces;

public interface IOfficeService
{
    public Task<Office> AddOfficeAsync(string address, string phoneNumber, CancellationToken cancellationToken = default);
    public Task<Office> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Office>> GetOffices(CancellationToken cancellationToken = default);
    public Task<Office> UpdateOfficeAsync(Guid id, UpdateOfficeDto dto, CancellationToken cancellationToken = default);
    public Task RemoveOfficeAsync(Guid id, CancellationToken cancellationToken = default);
    public void ChangeOfficeStatusToIsActive(Office office);
    public void ChangeOfficeStatusToIsNotActive(Office office);
    Task<Office> GetOfficeByAddressAsync(string address, CancellationToken cancellationToken);
}