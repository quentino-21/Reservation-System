using ReservationSystem.Domain.Enums;

namespace ReservationSystem.Domain.Dtos.Reservation;

public sealed record ReservationDto
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid ProductId { get; set; }
    
    public required string ProductName { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public required DateTimeOffset StartTime { get; set; }
}