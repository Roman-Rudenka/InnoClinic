using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common;

namespace Infrastructure.Repositories;

public class ReceptionRepository(AppDbContext context) : ProfileRepository<Receptionist>(context), IReceptionRepository
{
    
}