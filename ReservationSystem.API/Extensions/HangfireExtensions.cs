using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;
using Hangfire;
using ReservationSystem.Application.Services;

namespace ReservationSystem.API.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddHangfireServer();
        services.AddScoped<IRecurringJobManager, RecurringJobManager>();

        return services;
    }

    public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var reservationJobService = scope.ServiceProvider.GetRequiredService<ReservationJobService>();

        jobManager.AddOrUpdate(
            "update-reservation-status",
            () => reservationJobService.ExpireReservationsJob(),
            Cron.Daily);

        return app;
    }
}