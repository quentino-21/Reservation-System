using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Reservation;

namespace ReservationSystem.Domain.Interfaces;

public interface IReservationService
{
    Task<PaginatedResponseDto<ReservationDto>> GetReservationsAsync(PageQueryFilterDto filterDto, CancellationToken ct);
    Task<ReservationDto> GetReservationByIdAsync(Guid id, CancellationToken ct);
}