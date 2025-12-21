using Application.DTOs;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class OfficeService :  IOfficeService
{
    private readonly IOfficeRepository _officeRepository;

    public OfficeService(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }
    
    public async Task<Office> AddOfficeAsync(string address, string phoneNumber, CancellationToken cancellationToken = default)
    {
        // validate address function
        
        var officeData = new Office()
        {
            Address = address,
            RegistryPhoneNumber = phoneNumber,
            IsActive = false
        };
        await _officeRepository.AddOfficeAddressAsync(officeData, cancellationToken);
        await _officeRepository.SaveChangesAsync(cancellationToken);
        
        return officeData;
    }

    public async Task<Office> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        var existingOffice = await _officeRepository.GetAddressByIdAsync(id, cancellationToken);

        return existingOffice;
    }

    public async Task<IEnumerable<Office>> GetOffices(CancellationToken cancellationToken = default)
    {
        var offices = await _officeRepository.GetOfficeAddressesAsync(cancellationToken);
        
        return offices;
    }

    public async Task<Office> UpdateOfficeAsync(Guid id, UpdateOfficeDto dto, CancellationToken cancellationToken = default)
    {
        //var newAddress = dto.Address;
        // validate address function
        
        var office = await _officeRepository.GetAddressByIdAsync(id, cancellationToken);
        if (office is null)
        {
            throw new KeyNotFoundException($"Office not found");
        }

        if (!string.IsNullOrWhiteSpace(dto.Address))
        {
            office.Address = dto.Address;
        }

        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            office.RegistryPhoneNumber = dto.PhoneNumber;
        }

        if (dto.Status.HasValue)
        {
            office.IsActive = dto.Status.Value;
        }
        await _officeRepository.UpdateAddressAsync(office, cancellationToken);
        
        return office;
    }

    public async Task RemoveOfficeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _officeRepository.RemoveAddressAsync(id, cancellationToken);
        await _officeRepository.SaveChangesAsync(cancellationToken);
    }

    public void ChangeOfficeStatusToIsActive(Office office)
    {
        office.IsActive = true;
    }

    public void ChangeOfficeStatusToIsNotActive(Office office)
    {
        office.IsActive = false;
    }
}