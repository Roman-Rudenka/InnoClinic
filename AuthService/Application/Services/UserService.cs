using Application.AuthAdditions;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<IdentityResult> RegisterUserAsync(string email, string password, string phoneNumber, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = email,
            UserName = email,
            PhoneNumber = phoneNumber,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        return await _userManager.CreateAsync(user, password);
    }

    public async Task<User?> ValidateUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded ? user : null;
    }

    public async Task<AuthResult> GenerateTokensAsync(User user, CancellationToken cancellationToken)
    {
        if (user.Email == null)
        {
            throw new Exception("Email is null");
        }
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, cancellationToken);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}