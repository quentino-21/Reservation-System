using ReservationSystem.Domain.Common;

namespace ReservationSystem.Domain.Entities;

public sealed record Product : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required TimeSpan Duration { get; set; }
    public Double? Price { get; set; }
}