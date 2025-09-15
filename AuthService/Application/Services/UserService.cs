using System.Security.Claims;
using Application.AuthDTO;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> RegisterUserAsync(string email, string password, string phoneNumber, Roles role, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = email,
            UserName = email,
            PasswordHash = password,
            PhoneNumber = phoneNumber,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user,password);
        if (!result.Succeeded)
        {
            return null;
        }
        
        await _userManager.AddToRoleAsync(user, role.ToString());
        return result;
    }

    public async Task<RefreshTokensDTO?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return null;
        }

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, cancellationToken);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new RefreshTokensDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    public async Task<RefreshTokensDTO> RefreshTokensAsync(RefreshTokensDTO request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.ValidateAccessToken(request.AccessToken, cancellationToken);
        if (principal == null)
        {
            return null;
        }
    
        var userId = principal.FindFirstValue("id");
        if (userId == null || !Guid.TryParse(userId, out var guid))
        {
            return null;
        }
    
        var isValid = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken, guid, cancellationToken);
        if (!isValid)
        {
            return null;
        }
    
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
    
        var user = await _userRepository.GetUserByIdAsync(guid, cancellationToken);
        if (user == null)
        {
            return null;
        }
    
        return await GenerateTokensAsync(user, cancellationToken);
    }
    
    private async Task<RefreshTokensDTO> GenerateTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, cancellationToken);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
    
        return new RefreshTokensDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
