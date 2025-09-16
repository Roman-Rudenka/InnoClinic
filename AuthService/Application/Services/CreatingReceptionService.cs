using Application.Exceptions;
using Application.Interfaces;
using Application.Options;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class CreatingReceptionService(UserManager<User> userManager, IOptions<ReceptionUserOptions> options)
    : ICreatingReceptionService
{
    public async Task CreateReceptionAsync(CancellationToken cancellationToken = default) 
    { 
        var config = options.Value;
        
        var existing = await userManager.FindByEmailAsync(config.Email);
        if (existing != null)
        {
            return;
        }

        var user = new User 
        { 
            Email = config.Email, 
            UserName = config.Email, 
            PhoneNumber = config.PhoneNumber, 
            EmailConfirmed = true, 
            CreatedAt = DateTime.UtcNow 
        };

        var result = await userManager.CreateAsync(user, config.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Unable to create reception");
        }

        await userManager.AddToRoleAsync(user, nameof(Roles.Reception));
        }

}