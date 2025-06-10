using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Mappers;
using ReservationSystem.Application.Utils;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Reservation;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.Application.Services;

public class ClientReservationService : IClientReservationService
{
    private readonly IUserContextService _userContext;
    private readonly ReservationDbContext _context;

    public ClientReservationService(IUserContextService userContext, ReservationDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<PaginatedResponseDto<ReservationDto>> GetReservationsAsync(PageQueryFilterDto filterDto, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }

        var reservations = await _context.Reservations
            .Where(x => x.UserId == userId)
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
        var userId = _userContext.GetCurrentUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }
        
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
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
    
    public async Task<Guid> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }
        
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId, ct);

        if (product == null)
        {
            throw new BadRequestException("Product not found");
        }
        
        var reservation = new Reservation
        {
            UserId = userId.Value,
            ProductId = dto.ProductId,
            StartTime = dto.StartTime,
        };
        
        await _context.Reservations.AddAsync(reservation, ct);
        await _context.SaveChangesAsync(ct);
        
        return reservation.Id;
    }
    
    public async Task CancelReservationAsync(Guid id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }
        
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }

        if (reservation.Status != ReservationStatus.Pending || reservation.Status != ReservationStatus.Confirmed)
        {
            throw new BadRequestException("Reservation has to be Pending or Confirmed");
        }

        reservation.Status = ReservationStatus.Cancelled;
        
        await _context.SaveChangesAsync(ct);
    }
}