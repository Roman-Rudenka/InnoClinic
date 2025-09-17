using Application.AuthDTO;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber,Roles.Patient, cancellationToken);

        return Ok("Patient registered");
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber, Roles.Doctor,  cancellationToken);
        
        return Ok("Doctor registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var tokens = await userService.LoginAsync(request.Email, request.Password, cancellationToken);
        if (tokens?.AccessToken == null)
        {
            return Unauthorized();
        }
        
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
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokensDto request, CancellationToken cancellationToken = default)
    {
        var tokens = await userService.RefreshTokensAsync(request, cancellationToken);
        if (string.IsNullOrEmpty(tokens.AccessToken) || string.IsNullOrEmpty(tokens.RefreshToken))
        {
            return Unauthorized("Invalid token pair");
        }

        Response.Cookies.Append("access_token", tokens.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15),
            Path = "/",
            IsEssential = true
        });

        Response.Cookies.Append("refresh_token", tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/",
            IsEssential = true
        });

        return Ok("Token refreshed");
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        await userService.LogoutAsync(User, cancellationToken);
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");
        
        return Ok(new { message = "log out successful" });
    }
}



