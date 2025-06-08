using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Enums;

namespace ReservationSystem.Domain.Entities;

public sealed record Reservation : BaseEntity
{
    public required Guid UserId { get; set; }
    public required Guid ProductId { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public required DateTimeOffset StartTime { get; set; }
    
    public User? User { get; set; }
    public Product? Product { get; set; }
}

public class ReservationConfiguration : BaseEntityConfiguration<Reservation>
{
    public override void Configure(EntityTypeBuilder<Reservation> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}