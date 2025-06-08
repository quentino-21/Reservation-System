using FluentValidation;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationSystem.Application.Common;
using ReservationSystem.Application.Services;
using ReservationSystem.Application.Middleware;
using ReservationSystem.Application.Validators;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;

namespace ReservationSystem.Application;

public static class Setup
{
    public static void AddApplicationLogic(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        
        var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();
        if (jwtOptions == null)
        {
            throw new InvalidOperationException("JWTSettings not found in configuration");
        }

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddValidatorsFromAssemblyContaining<PageQueryFilterDtoValidator>();

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        
        builder.Services.AddScoped<ReservationJobService>();
        
        builder.Services.AddSingleton(jwtOptions);

        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IAdminProductService, AdminProductService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        builder.Services.AddScoped<IAdminReservationService, AdminReservationService>();
        builder.Services.AddScoped<IClientReservationService, ClientReservationService>();
    }
}