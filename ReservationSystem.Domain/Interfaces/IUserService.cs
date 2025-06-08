using ReservationSystem.Domain.Dtos.Users;

namespace ReservationSystem.Domain.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserAsync(CancellationToken ct);

    Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken ct);
    
    Task UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken ct);
}