using Application.AuthDTO;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> RegisterUserAsync(string email, string password, string phoneNumber, Roles role,  CancellationToken cancellationToken); 
    public Task<RefreshTokensDto?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    public Task LogoutAsync(string accessToken , CancellationToken cancellationToken);
    public Task<RefreshTokensDto> RefreshTokensAsync(RefreshTokensDto request, CancellationToken cancellationToken);
    public Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);
    public Task<IdentityResult> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken);
}