using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Mappers;
using ReservationSystem.Application.Utils;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Reservation;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.Application.Services;

public class ReservationService : IReservationService
{
    private readonly ReservationDbContext _context;

    public ReservationService(ReservationDbContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedResponseDto<ReservationDto>> GetReservationsAsync(PageQueryFilterDto filterDto, CancellationToken ct)
    {
        var baseQuery = _context.Reservations.AsQueryable();
        
        var count = baseQuery.Count();

        var items = await baseQuery
            .Select(x => ReservationMappers.MapToReservationDto(x))
            .Paginate(filterDto.PageNumber, filterDto.PageSize)
            .ToListAsync(ct);

        var result = new PaginatedResponseDto<ReservationDto>(items, count, filterDto.PageNumber, filterDto.PageSize);
        
        return result;
    }

    public async Task<ReservationDto> GetReservationByIdAsync(Guid id, CancellationToken ct)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }
        
        var reservationDto = ReservationMappers.MapToReservationDto(reservation);

        return reservationDto;
    }
}