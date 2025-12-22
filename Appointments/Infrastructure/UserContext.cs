using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null) 
            throw new UnauthorizedAccessException("Context user is null");

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier) 
                      ?? user.FindFirst("sub")
                      ?? user.FindFirst("id");

        if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
        {
            return id;
        }

        throw new UnauthorizedAccessException("User ID claim not found in token.");
    }

    public string GetUserRole()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null) return "Guest";

        return user.FindFirst(ClaimTypes.Role)?.Value 
               ?? user.FindFirst("role")?.Value 
               ?? "Guest";
    }
}