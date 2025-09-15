using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public class RoleSeeder : IRoleSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var allowedRoles = Enum.GetNames(typeof(Roles));

        foreach (var roleName in allowedRoles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        var extraRoles = allRoles.Except(allowedRoles);

        foreach (var extra in extraRoles)
        {
            var role = await _roleManager.FindByNameAsync(extra);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }
    }
}
