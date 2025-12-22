using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PatientRepository(AppDbContext context) : ProfileRepository<Patient>(context), IPatientRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Patient?> GetByNameAsync(string firstName, string lastName, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => 
                    p.FirstName == firstName && 
                    p.LastName == lastName, 
                cancellationToken);
    }
}