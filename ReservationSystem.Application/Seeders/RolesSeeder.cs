using Microsoft.AspNetCore.Identity;
using ReservationSystem.Application.Common;

namespace ReservationSystem.Application.Seeders;

public static class RolesSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in Roles.RolesNames)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }
            
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}