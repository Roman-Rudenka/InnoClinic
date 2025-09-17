using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;
using Application.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Middlewares;

public class JwtControllingMiddleware(
    RequestDelegate next,
    IDistributedCache cache,
    ILogger<JwtControllingMiddleware> logger,
    IOptions<JwtOptions> jwtOptions,
    IOptions<RedisOptions> redisOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly RedisOptions _redisOptions = redisOptions.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            await next(context);
            return;
        }

        var handler = new JwtSecurityTokenHandler();
        ClaimsPrincipal? principal;

        try
        {
            principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret))
            }, out _);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Invalid JWT token");
            throw new UnauthorizedException("Invalid token");
        }

        var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (!string.IsNullOrEmpty(jti))
        {
            var key = $"{_redisOptions.InstanceName}revoked:{jti}";
            var revoked = await cache.GetStringAsync(key);

            if (revoked is not null)
            {
                logger.LogInformation("Blocked request with revoked token: {Jti}", jti);
                throw new UnauthorizedException("Token has been revoked");
            }
        }

        context.User = principal;
        await next(context);
    }
}
