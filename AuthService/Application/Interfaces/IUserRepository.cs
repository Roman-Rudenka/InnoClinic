using Domain.Models;

namespace Application.Interfaces;

public interface IUserRepository
{
    public Task AddUserAsync(User user, CancellationToken cancellationToken);
    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    public Task<User?>  GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task<User> UpdateEmailStatusAsync(string email, CancellationToken cancellationToken);
}