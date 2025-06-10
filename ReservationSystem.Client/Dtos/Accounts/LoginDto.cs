namespace ReservationSystem.Client.Dtos.Accounts;

public sealed record LoginDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}