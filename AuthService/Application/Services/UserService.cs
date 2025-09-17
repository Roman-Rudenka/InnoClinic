using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.AuthDTO;
using Application.Exceptions;
using Application.Interfaces;
using Application.Options;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class UserService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenService tokenService,
    IUserRepository userRepository,
    IDistributedCache cache,
    IOptions<RedisOptions> redisOptions)
    : IUserService
{
    private readonly RedisOptions _redisOptions = redisOptions.Value;
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

        var result = await userManager.CreateAsync(user,password);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Invalid data");
        }
        
        await userManager.AddToRoleAsync(user, role.ToString());
        return result;
    }

    public async Task<RefreshTokensDto?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Invalid password");
        }

        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email!, cancellationToken);
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new RefreshTokensDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task LogoutAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var jti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var userId = user.FindFirst("id")?.Value;
        
        if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userId))
        {
            throw new BadRequestException("Invalid token");
        }

        var key = $"{_redisOptions.InstanceName}revoked:{jti}";

        await cache.SetStringAsync(key, "true", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        }, cancellationToken);
        var userIdGuid = Guid.Parse(userId);
        
        await tokenService.RevokeRefreshTokenByUserIdAsync(userIdGuid, cancellationToken);
    }
    
    public async Task<RefreshTokensDto> RefreshTokensAsync(RefreshTokensDto request, CancellationToken cancellationToken)
    {
        var principal = tokenService.ValidateAccessToken(request.AccessToken, cancellationToken);
        if (principal == null)
        {
            throw new BadRequestException("Invalid token");
        }
    
        var userId = principal.FindFirstValue("id");
        if (userId == null || !Guid.TryParse(userId, out var guid))
        {
            throw new BadRequestException("Invalid token");
        }
    
        var isValid = await tokenService.ValidateRefreshTokenAsync(request.RefreshToken, guid, cancellationToken);
        if (!isValid)
        {
            throw new BadRequestException("Invalid token");
        }
    
        await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
    
        var user = await userRepository.GetUserByIdAsync(guid, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
    
        return await GenerateTokensAsync(user, cancellationToken);
    }
    
    private async Task<RefreshTokensDto> GenerateTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email!, cancellationToken);
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
    
        return new RefreshTokensDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
