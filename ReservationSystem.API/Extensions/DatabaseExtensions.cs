using ReservationSystem.Domain.Entities;
using ReservationSystem.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ReservationSystem.API.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReservationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
        
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ReservationDbContext>()
            .AddDefaultTokenProviders();
    }
}