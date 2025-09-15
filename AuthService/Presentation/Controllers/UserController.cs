using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService, ITokenService tokenService, IUserRepository userRepository) : ControllerBase
{
    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber, cancellationToken);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        return Ok("Patient registered");
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber, cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        return Ok("Doctor registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userService.ValidateUserAsync(request.Email, request.Password, cancellationToken);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var tokens = await userService.GenerateTokensAsync(user, cancellationToken);

        Response.Cookies.Append("access_token", tokens.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("refresh_token", tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok("Logged in");
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken = default)
    {
        var principal = tokenService.ValidateAccessToken(request.AccessToken, cancellationToken);
        if (principal == null)
        {
            return Unauthorized("Invalid access token");
        }

        var userId = principal.FindFirstValue("id");
        if (userId == null || !Guid.TryParse(userId, out var guid))
        {
            return Unauthorized("Invalid user ID");
        }

        var isValid = await tokenService.ValidateRefreshTokenAsync(request.RefreshToken, guid, cancellationToken);
        if (!isValid)
        {
            return Unauthorized("Invalid refresh token");
        }

        await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
        var user = await userRepository.GetUserByIdAsync(guid, cancellationToken);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        var tokens = await userService.GenerateTokensAsync(user, cancellationToken);

        Response.Cookies.Append("access_token", tokens.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,  
            Expires = DateTime.UtcNow.AddMinutes(15),
            Path = "/",     
            IsEssential = true 
        });
        Response.Cookies.Append("refresh_token", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/",
                IsEssential = true
            }
        );

        return Ok("Token refreshed");
    }
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "log out successful" });
    }
}






