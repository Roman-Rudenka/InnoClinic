using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DoctorRepository(AppDbContext context) : ProfileRepository<Doctor>(context), IDoctorRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Doctor>> GetBySpecializationIdAsync(Guid specializationId, CancellationToken cancellationToken)
    {
        return await _context.Doctors
            .Where(d => d.SpecializationId == specializationId)
            .ToListAsync(cancellationToken);
    }
}