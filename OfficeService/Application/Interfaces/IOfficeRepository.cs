using Domain.Models;

namespace Application.Interfaces;

public interface IOfficeRepository
{
    public Task AddOfficeAddressAsync(Office office, CancellationToken cancellationToken = default);
    public Task<Office> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Office>> GetOfficeAddressesAsync(CancellationToken cancellationToken = default);
    public Task<Office> UpdateAddressAsync(Office office, CancellationToken cancellationToken = default);
    public Task RemoveAddressAsync(Guid id, CancellationToken cancellationToken = default);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}