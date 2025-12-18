using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common;

namespace Infrastructure.Repositories;

public class PatientRepository : ProfileRepository<Patient>, IPatientRepository 
{
    public PatientRepository(AppDbContext context) : base(context)
    {
        
    }
}