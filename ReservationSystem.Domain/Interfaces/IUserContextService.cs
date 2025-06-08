using System.Security.Claims;

namespace ReservationSystem.Domain.Interfaces;

public interface IUserContextService
{
    ClaimsPrincipal? GetCurrentUser();
    Guid? GetCurrentUserId();
}