using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.Application.Services;

public class AdminReservationService : IAdminReservationService
{
    private readonly ReservationDbContext _context;

    public AdminReservationService(ReservationDbContext context)
    {
        _context = context;
    }
    
    public async Task ConfirmReservationAsync(Guid id, CancellationToken ct)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }

        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new BadRequestException("Reservation could not be confirmed");
        }
        
        reservation.Status = ReservationStatus.Confirmed;
        
        await _context.SaveChangesAsync(ct);
    }

    public async Task RejectReservationAsync(Guid id, CancellationToken ct)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }
        
        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new BadRequestException("Reservation could not be rejected");
        }
        
        reservation.Status = ReservationStatus.Rejected;
        
        await _context.SaveChangesAsync(ct);
    }

    public async Task CompleteReservationAsync(Guid id, CancellationToken ct)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (reservation == null)
        {
            throw new NotFoundException("Reservation not found");
        }
        
        var now = DateTimeOffset.UtcNow;

        if (now < reservation.StartTime || reservation.Status != ReservationStatus.Confirmed)
        {
            throw new BadRequestException("Reservation could not be completed");
        }
        
        reservation.Status = ReservationStatus.Completed;
        
        await _context.SaveChangesAsync(ct);
    }
}