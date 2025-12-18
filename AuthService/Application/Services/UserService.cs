using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Application.AuthDTO;
using Application.Exceptions;
using Application.Interfaces;
using Application.Options;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class UserService(
    UserManager<User> userManager,
    ITokenService tokenService,
    IEmailService emailService,
    IDistributedCache cache,
    IOptions<RedisOptions> redisOptions)
    : IUserService
{
    public async Task<User> RegisterUserAsync(string email, string password, string phoneNumber, Roles role, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = email,
            UserName = email,
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
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink = $"localhost:5162/api/auth/confirm-email?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
        
        await emailService.SendEmailAsync(user.Email!, "Confirm your email (Resend)", confirmationLink, cancellationToken);
        
        return user;
    }
    
    public async Task<RefreshTokensDto?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("Invalid email or password");
        }
    
        var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            throw new NotFoundException("Invalid email or password");
        }
        
        return  await GenerateTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(string  accessToken, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromAccessToken(accessToken);
        if (principal == null)
        {
            throw new BadRequestException("Invalid token");
        }
        
        var userId = principal.FindFirstValue("id");
        if (userId == null)
        {
            throw new BadRequestException("Invalid token");
        }

        await tokenService.RevokeRefreshTokenByUserIdAsync(Guid.Parse(userId), cancellationToken);
        
        var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (!string.IsNullOrEmpty(jti))
        {
            var key = $"{redisOptions.Value}revoked:{jti}";
            await cache.SetStringAsync(key, "true", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            }, cancellationToken);
        }
    }
    
    
    public async Task<RefreshTokensDto> RefreshTokensAsync(RefreshTokensDto request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            throw new BadRequestException("Invalid token");
        }
    
        var userId = principal.FindFirstValue("id");
        if (userId == null || !Guid.TryParse(userId, out var id))
        {
            throw new BadRequestException("Invalid token");
        }
    
        var isValid = await tokenService.ValidateRefreshTokenAsync(request.RefreshToken, id, cancellationToken);
        if (!isValid)
        {
            await tokenService.RevokeRefreshTokenByUserIdAsync(id, cancellationToken);
            throw new BadRequestException("Invalid token");
        }
    
        await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, id, cancellationToken);
    
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
    
        return await GenerateTokensAsync(user, cancellationToken);
    }
    
    public async Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        return await userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<IdentityResult> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("User not found"); 
        }

        if (user.EmailConfirmed)
        {
            throw new  BadRequestException("Email already confirmed");
        }
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"localhost:5162/api/auth/confirm-email?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
        
        try
        {
            await emailService.SendEmailAsync(user.Email!, "Confirm your email (Resend)", confirmationLink, cancellationToken);
            
            return IdentityResult.Success;
        }
        catch (Exception)
        {
            throw new BadRequestException("Invalid email");
        }
    }

    private async Task<RefreshTokensDto> GenerateTokensAsync(User user, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(user);
        
        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new RefreshTokensDto(accessToken, refreshToken);
    }
}
