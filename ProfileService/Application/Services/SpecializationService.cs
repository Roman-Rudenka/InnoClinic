using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class SpecializationService(ISpecializationRepository specializationRepository) : ISpecializationService
{
    public async Task CreateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        var existing = await specializationRepository.GetByNameAsync(specialization.SpecializationName, cancellationToken);
        if (existing != null)
        {
             throw new InvalidOperationException("Specialization already exists.");
        }

        await specializationRepository.AddAsync(specialization, cancellationToken);
        await specializationRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Specialization> GetSpecializationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var spec = await specializationRepository.GetByIdAsync(id, cancellationToken);
        if (spec == null) throw new KeyNotFoundException("Specialization not found.");
        return spec;
    }

    public async Task<Specialization> GetSpecializationByNameAsync(string name, CancellationToken cancellationToken)
    {
        var spec = await specializationRepository.GetByNameAsync(name, cancellationToken);
        if (spec == null) throw new KeyNotFoundException("Specialization not found.");
        return spec;
    }

    public async Task<IEnumerable<Specialization>> GetSpecializationsAsync(CancellationToken cancellationToken)
    {
        return await specializationRepository.GetAllAsync(cancellationToken);
    }

    public async Task UpdateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        await specializationRepository.UpdateAsync(specialization, cancellationToken);
        await specializationRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSpecializationAsync(Guid id, CancellationToken cancellationToken)
    {
        var spec = await specializationRepository.GetByIdAsync(id, cancellationToken);
        if (spec == null) throw new KeyNotFoundException("Specialization not found.");

        await specializationRepository.DeleteAsync(spec, cancellationToken);
        await specializationRepository.SaveChangesAsync(cancellationToken);
    }
}