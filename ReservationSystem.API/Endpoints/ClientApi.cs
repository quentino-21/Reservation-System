using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Reservation;
using ReservationSystem.Domain.Interfaces;

namespace ReservationSystem.API.Endpoints;

public static class ClientApi
{
    public static IEndpointRouteBuilder MapClientApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clients")
            .WithTags("Clients")
            .RequireAuthorization("Client")
            .WithOpenApi();

        group.MapGet("/reservations", async (IClientReservationService clientReservationService, [AsParameters] PageQueryFilterDto filterDto, CancellationToken ct) =>
        {
            var result = await clientReservationService.GetReservationsAsync(filterDto, ct);

            return Results.Ok(result);
        })
        .Produces<PaginatedResponseDto<ReservationDto>>(StatusCodes.Status200OK, "application/json");
        
        group.MapGet("/reservations/{id:guid}", async (IClientReservationService clientReservationService, Guid id, CancellationToken ct) =>
        {
            var result = await clientReservationService.GetReservationByIdAsync(id, ct);
    
            return Results.Ok(result);
        })
        .Produces<ReservationDto>(StatusCodes.Status200OK, "application/json");

        group.MapPost("/reservations", async (IClientReservationService clientReservationService, CreateReservationDto dto, CancellationToken ct) =>
        {
            var id = await clientReservationService.CreateReservationAsync(dto, ct);

            return Results.Created($"/api/clients/reservations/{id}", id);
        })
        .Produces(StatusCodes.Status201Created);
        
        group.MapPost("/reservations/{id:guid}/cancel", async (IClientReservationService clientReservationService, Guid id, CancellationToken ct) =>
        {
            await clientReservationService.CancelReservationAsync(id, ct);

            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK);
        
        return app;
    }
}