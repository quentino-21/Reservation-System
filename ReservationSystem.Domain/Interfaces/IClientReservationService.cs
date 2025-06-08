using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Dtos.Reservation;

namespace ReservationSystem.Domain.Interfaces;

public interface IClientReservationService
{
    Task<PaginatedResponseDto<ReservationDto>> GetReservationsAsync(PageQueryFilterDto filterDto, CancellationToken ct);
    Task<ReservationDto> GetReservationByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct);
    Task CancelReservationAsync(Guid id, CancellationToken ct);
}