using Microsoft.AspNetCore.Identity;
using ReservationSystem.Application.Seeders;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.API.Extensions;

public static class SeedingExtensions
{
    public static async Task SeedAsync(this WebApplication app, IConfiguration configuration)
    {
        await using var scope = app.Services.CreateAsyncScope();

        await using var context = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        
        await context.Database.EnsureCreatedAsync();
        
        await RolesSeeder.SeedAsync(roleManager);
    }
}