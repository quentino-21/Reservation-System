using ReservationSystem.Domain.Dtos.Reservation;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Application.Mappers;

public class ReservationMappers
{
    public static ReservationDto MapToReservationDto(Reservation reservation)
    {
        var reservationDto = new ReservationDto
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            ProductId = reservation.ProductId,
            Status = reservation.Status,
            StartTime = reservation.StartTime
        };
        return reservationDto;
    }
}