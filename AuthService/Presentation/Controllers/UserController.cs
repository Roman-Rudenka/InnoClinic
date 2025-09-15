using System.Security.Claims;
using Application.AuthDTO;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Presentation.Requests;

namespace Presentation.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber,Roles.Patient, cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        return Ok("Patient registered");
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber, Roles.Doctor,  cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        return Ok("Doctor registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var tokens = await userService.LoginAsync(request.Email, request.Password, cancellationToken);
    
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
    public async Task<IActionResult> Refresh([FromBody] RefreshTokensDTO request, CancellationToken cancellationToken = default)
    {
        var tokens = await userService.RefreshTokensAsync(request, cancellationToken);
        if (tokens == null)
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

    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "log out successful" });
    }
}






