using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager) : IRoleSeeder
{
    public async Task SeedAsync()
    {
        var allowedRoles = Enum.GetNames(typeof(Roles));

        foreach (var roleName in allowedRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        var allRoles = roleManager.Roles.Select(r => r.Name).ToList();
        var extraRoles = allRoles.Except(allowedRoles);

        foreach (var extra in extraRoles)
        {
            if (extra != null)
            {
                var role = await roleManager.FindByNameAsync(extra);
                if (role != null)
                {
                    await roleManager.DeleteAsync(role);
                }
            }
        }
    }
}
