using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Mappers;
using ReservationSystem.Application.Utils;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Reservation;
using ReservationSystem.Domain.Entities;
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
        var reservations = await _context.Reservations
            .Include(r => r.Product)
            .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
            .Take(filterDto.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        var items = reservations.Select(r =>
            ReservationMappers.MapToReservationDto(r, r.Product!)
        ).ToList();

        var count = await _context.Reservations.CountAsync(ct);

        return new PaginatedResponseDto<ReservationDto>(items, filterDto.PageNumber, filterDto.PageSize, count);
    }

    public async Task<ReservationDto> GetReservationByIdAsync(Guid id, CancellationToken ct)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }
        
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == reservation.ProductId, ct);
        if (product == null)
        {
            throw new NotFoundException("Product not found for this reservation");
        }
        
        var reservationDto = ReservationMappers.MapToReservationDto(reservation, product);

        return reservationDto;
    }
}