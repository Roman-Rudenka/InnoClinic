using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly JwtOptions _jwtOptions;
        private readonly SymmetricSecurityKey _key;
        
        public TokenService(IOptions<JwtOptions> jwtOptions, ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _jwtOptions = jwtOptions.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        }

        public string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles)
        {
            var jti = Guid.CreateVersion7().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim("id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes),
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

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
            var storedToken = await _tokenRepository.GetRefreshTokenAsync(token, userId, cancellationToken);
            
            if (storedToken == null) return false;

            return storedToken.ExpiresAt > DateTime.UtcNow;
        }

        public ClaimsPrincipal? GetPrincipalFromAccessToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = _key,
                    ClockSkew = TimeSpan.Zero 
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = _key,
                    ClockSkew = TimeSpan.Zero 
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task RevokeRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken)
        {
            var storedToken = await _tokenRepository.GetRefreshTokenAsync(token, userId, cancellationToken);
            
            if (storedToken != null)
            { 
                await _tokenRepository.DeleteRefreshTokenAsync(storedToken);
                await _tokenRepository.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RevokeRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _tokenRepository.DeleteRefreshTokenByUserIdAsync(userId, cancellationToken);
        }
    }