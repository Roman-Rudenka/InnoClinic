using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ResultRepository(AppointmentsDbContext context) : IResultRepository
{
    public async Task AddAsync(Result result, CancellationToken ct)
    {
        await context.Results.AddAsync(result, ct);
    }

    public async Task<Result?> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken ct)
    {
        return await context.Results.AsNoTracking().FirstOrDefaultAsync(r => r.AppointmentId == appointmentId, ct);
    }
}