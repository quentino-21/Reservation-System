using ReservationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Common;

namespace ReservationSystem.Infrastructure.Contexts;

public class ReservationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(BaseEntityConfiguration<>).Assembly);
    }
}