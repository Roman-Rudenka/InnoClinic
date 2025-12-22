using System.Net.Http.Json;
using Application.DTO;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class ReceptionService(IReceptionRepository receptionRepository) : IReceptionService
{
    public async Task CreateReceptionAsync(
        string firstName, 
        string lastName, 
        string? middleName, 
        DateOnly birthDate,
        Guid accountId, 
        Guid officeId,
        CancellationToken cancellationToken)
    {
        var receptionist = new Receptionist
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = birthDate,
            AccountId = accountId,
            IsLinkedToAccount = true,
            
            OfficeId = officeId
        };

        await receptionRepository.AddAsync(receptionist, cancellationToken);
        await receptionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Receptionist> GetReceptionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var receptionist = await receptionRepository.GetByIdAsync(id, cancellationToken);
        if (receptionist == null) throw new KeyNotFoundException("Receptionist not found.");
        return receptionist;
    }

    public async Task UpdateReceptionAsync(Guid id, string firstName, string lastName, string? middleName, DateOnly birthDate, CancellationToken cancellationToken)
    {
        var receptionist = await receptionRepository.GetByIdAsync(id, cancellationToken);
        if (receptionist == null) throw new KeyNotFoundException($"Receptionist not found.");

        receptionist.FirstName = firstName;
        receptionist.LastName = lastName;
        receptionist.MiddleName = middleName;
        receptionist.BirthDate = birthDate;

        await receptionRepository.UpdateAsync(receptionist, cancellationToken);
        await receptionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteReceptionAsync(Guid id, CancellationToken cancellationToken)
    {
        var receptionist = await receptionRepository.GetByIdAsync(id, cancellationToken);
        if (receptionist == null) throw new KeyNotFoundException($"Receptionist not found.");

        await receptionRepository.DeleteAsync(receptionist, cancellationToken);
        await receptionRepository.SaveChangesAsync(cancellationToken);
    }
}