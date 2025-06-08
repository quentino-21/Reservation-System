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

        var baseQuery = _context.Reservations.AsQueryable()
            .Where(x => x.UserId == userId);
        
        var count = await baseQuery.CountAsync(ct);
        
        var items = await baseQuery
            .Select(x => ReservationMappers.MapToReservationDto(x))
            .Paginate(filterDto.PageNumber, filterDto.PageSize)
            .ToListAsync(ct);
        
        var result = new PaginatedResponseDto<ReservationDto>(items, filterDto.PageNumber, filterDto.PageSize, count);
        
        return result;
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
        
        var reservationDto = ReservationMappers.MapToReservationDto(reservation);
        
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

        reservation.Status = ReservationStatus.Cancelled;
        
        await _context.SaveChangesAsync(ct);
    }
}