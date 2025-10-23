using Application.AuthDTO;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService) : ControllerBase
{
    private readonly CookieOptions _accessTokenCookieOptions = new()
    {
        HttpOnly = true,
        Secure = false,
        Expires = DateTime.UtcNow.AddMinutes(15),
        Path = "/"
    };
    
    private readonly CookieOptions _refreshTokenCookieOptions = new()
    {
        HttpOnly = true,
        Secure = false,
        Expires = DateTime.UtcNow.AddDays(7),
        Path = "/"
    };
    
    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber,Roles.Patient, cancellationToken);

        return Created();
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        await userService.RegisterUserAsync(request.Email, request.Password, request.PhoneNumber, Roles.Doctor,  cancellationToken);
        
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var tokens = await userService.LoginAsync(request.Email, request.Password, cancellationToken);

            if (tokens == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            Response.Cookies.Append("access_token", tokens.AccessToken, _accessTokenCookieOptions);
            Response.Cookies.Append("refresh_token", tokens.RefreshToken, _refreshTokenCookieOptions);

            return Ok("Logged in");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken = default)
    {
        var accessToken = Request.Cookies["access_token"];
        var refreshToken = Request.Cookies["refresh_token"];
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Invalid token pair");
        }

        try
        {
            var refreshTokensDto = new RefreshTokensDto(accessToken, refreshToken);
            var tokens = await userService.RefreshTokensAsync(refreshTokensDto, cancellationToken);

            Response.Cookies.Append("access_token", tokens.AccessToken, _accessTokenCookieOptions);
            Response.Cookies.Append("refresh_token", tokens.RefreshToken, _refreshTokenCookieOptions);

            return Ok("Token refreshed");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        var accessToken = Request.Cookies["access_token"];
        
        if (accessToken != null)
        {
            await userService.LogoutAsync(accessToken, cancellationToken);
            
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");
            
            return Ok("Logged out");
        }
        
        return Unauthorized();
    }
    
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(userId.ToString()) || string.IsNullOrWhiteSpace(token))
        {
            throw new BadRequestException("Unable to confirm email");
        }

        var result = await userService.ConfirmEmailAsync(userId, token);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Unable to confirm email");
        }

        return Ok(new { message = "Email confirmed successfully" });
    }
    
    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await userService.ResendConfirmationEmailAsync(request.Email, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Confirmation email resent. Please check your inbox." });
    }
}



