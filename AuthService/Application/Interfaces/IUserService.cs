using Application.AuthAdditions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> RegisterUserAsync(string email, string password, string phoneNumber, CancellationToken cancellationToken);
    public Task<User?> ValidateUserAsync(string email, string password, CancellationToken cancellationToken);
    public Task<AuthResult> GenerateTokensAsync(User user, CancellationToken cancellationToken);
}