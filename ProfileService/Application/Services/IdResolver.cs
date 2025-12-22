using System.Net.Http.Json;
using Application.DTO;
using Application.Interfaces;

namespace Application.Services;

public class IdResolverService(
    HttpClient httpClient, 
    ISpecializationRepository specializationRepository) : IIdResolverService
{
    public async Task<Guid> ResolveOfficeIdAsync(string address, CancellationToken ct)
    {
        var response = await httpClient.GetFromJsonAsync<OfficeDto>(
            $"http://localhost:5298/api/offices/by-address?address={address}", ct);

        if (response == null) 
            throw new KeyNotFoundException($"Office not found");

        return response.Id;
    }

    public async Task<Guid> ResolveSpecializationIdAsync(string name, CancellationToken ct)
    {
        var spec = await specializationRepository.GetByNameAsync(name, ct);
        
        if (spec == null) 
            throw new KeyNotFoundException("Specialization not found");

        return spec.Id;
    }
}
