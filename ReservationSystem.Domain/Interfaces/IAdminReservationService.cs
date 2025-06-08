namespace ReservationSystem.Domain.Interfaces;

public interface IAdminReservationService
{
    Task ConfirmReservationAsync(Guid id, CancellationToken ct);
    Task RejectReservationAsync(Guid id, CancellationToken ct);
    
    Task CompleteReservationAsync(Guid id, CancellationToken ct);
}