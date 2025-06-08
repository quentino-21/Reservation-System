namespace ReservationSystem.Domain.Dtos.Accounts;

public sealed record LoginResponseDto
{
    public required string JwtToken { get; init; }
    public required string RefreshToken { get; init; }
}