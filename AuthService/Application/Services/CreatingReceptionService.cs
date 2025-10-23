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
    public async Task CreateReceptionAsync() 
    { 
        var existing = await userManager.FindByEmailAsync(options.Value.Email);
        
        if (existing != null)
        {
            return;
        }

        var user = new User 
        { 
            Email = options.Value.Email, 
            UserName = options.Value.Email, 
            PhoneNumber = options.Value.PhoneNumber, 
            EmailConfirmed = true, 
            CreatedAt = DateTime.UtcNow 
        };

        var result = await userManager.CreateAsync(user, options.Value.Password);
        if (!result.Succeeded)
        {
            throw new Exception("unable to create reception");
        }

        await userManager.AddToRoleAsync(user, nameof(Roles.Reception));
    }
}