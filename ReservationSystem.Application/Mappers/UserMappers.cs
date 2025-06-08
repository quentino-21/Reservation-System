using ReservationSystem.Domain.Dtos.Users;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Application.Mappers;

public static class UserMappers
{
    public static UserDto MapToUserDto(User user)
    {
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email!
        };
        
        return userDto;
    }
}