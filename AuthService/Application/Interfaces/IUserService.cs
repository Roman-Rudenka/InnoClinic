using Application.AuthDTO;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> RegisterUserAsync(string email, string password, string phoneNumber, Roles role,  CancellationToken cancellationToken); 
    public Task<RefreshTokensDTO?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    public Task<RefreshTokensDTO> RefreshTokensAsync(RefreshTokensDTO request, CancellationToken cancellationToken); 
}