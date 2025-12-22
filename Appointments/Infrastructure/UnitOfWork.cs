using Application.Interfaces;

namespace Infrastructure;

public class UnitOfWork(AppointmentsDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await context.SaveChangesAsync(ct);
    }
}