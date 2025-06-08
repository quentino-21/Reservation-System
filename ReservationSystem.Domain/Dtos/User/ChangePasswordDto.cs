﻿namespace ReservationSystem.Domain.Dtos.Users;

public sealed record ChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmPassword { get; set; }
}