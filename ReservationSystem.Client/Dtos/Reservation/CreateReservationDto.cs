namespace ReservationSystem.Client.Dtos.Reservation;

public sealed record CreateReservationDto
{
    public required Guid ProductId { get; set; }
    public required DateTimeOffset StartTime { get; set; }
}