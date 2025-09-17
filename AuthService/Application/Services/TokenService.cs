using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly ITokenRepository _tokenRepository;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IOptions<JwtOptions> jwtOptions, ITokenRepository tokenRepository)
    {
        _jwtOptions = jwtOptions.Value;
        _tokenRepository = tokenRepository;

        if (string.IsNullOrWhiteSpace(_jwtOptions.Secret))
        {
            throw new InvalidOperationException("JWT secret is missing in configuration.");
        }

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
    }

    public string GenerateAccessToken(Guid userId, string email, CancellationToken cancellationToken = default)
    {
        var jti = Guid.CreateVersion7();
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim("id", userId.ToString()),
            new Claim("email", email)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays)
        };

        await _tokenRepository.CreateRefreshTokenAsync(refreshToken, cancellationToken);
        await _tokenRepository.SaveChangesAsync(cancellationToken);
        return token;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default)
    {
        var stored = await _tokenRepository.GetRefreshTokenAsync(token, userId, cancellationToken);
        return stored != null && stored.ExpiresAt > DateTime.UtcNow;
    }

    public ClaimsPrincipal? ValidateAccessToken(string token, CancellationToken cancellationToken = default)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = _key
            }, out _);
        }
        catch
        {
            return null;
        }
    }

    public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        await _tokenRepository.DeleteRefreshTokenAsync(token, cancellationToken);
        await  _tokenRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _tokenRepository.DeleteRefreshTokenByUserIdAsync(userId, cancellationToken);
        await _tokenRepository.SaveChangesAsync(cancellationToken);
    }
}