using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OfficeRepository(AppDbContext context) : IOfficeRepository
{
    public async Task AddOfficeAddressAsync(Office office, CancellationToken cancellationToken = default)
    {
        await context.Offices.AddAsync(office, cancellationToken);
    }

    public async Task<Office> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Offices.FirstAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Office>> GetOfficeAddressesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Offices.ToListAsync(cancellationToken);
    }

    public async Task<Office> UpdateAddressAsync(Office office, CancellationToken cancellationToken = default)
    { 
        context.Offices.Update(office);
        await context.SaveChangesAsync(cancellationToken);
        
        return  office;
    }

    public async Task RemoveAddressAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.Offices.FindAsync(id, cancellationToken);
        if (result is not null)
        {
            context.Offices.Remove(result);
        }
        throw new ApplicationException("Office not found");
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}