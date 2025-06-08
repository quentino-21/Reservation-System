namespace ReservationSystem.Domain.Exceptions;

public sealed class ForbidException(string message) : Exception(message);