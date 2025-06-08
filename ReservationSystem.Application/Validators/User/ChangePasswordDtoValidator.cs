﻿using FluentValidation;
using ReservationSystem.Domain.Dtos.Users;

namespace ReservationSystem.Application.Validators.User;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("Passwords can not match");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match");
        
    }
}