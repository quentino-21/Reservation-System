using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.Application.Services;

public class ReservationJobService
{
    private readonly ReservationDbContext _context;

    public ReservationJobService(ReservationDbContext context)
    {
        _context = context;
    }

    public async Task ExpireReservationsJob()
    {
        var now = DateTimeOffset.UtcNow;

        var baseQuery = _context.Reservations
            .Where(x => (x.Status == ReservationStatus.Pending && x.StartTime < now) || (x.Status == ReservationStatus.Confirmed && x.StartTime < now.AddDays(1)));
        
        var count = await baseQuery.CountAsync();
        
        var items = await baseQuery
            .ToListAsync();

        foreach (var item in items)
        {
            item.Status = ReservationStatus.Expired;
        }

        if (count > 0)
        {
            await _context.SaveChangesAsync();
        }
    }
}
