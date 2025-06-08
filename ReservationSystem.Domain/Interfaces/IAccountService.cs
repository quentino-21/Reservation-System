using ReservationSystem.Domain.Dtos.Accounts;

namespace ReservationSystem.Domain.Interfaces;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken ct);
    Task RegisterAsync(RegisterDto dto, CancellationToken ct);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken ct);
}