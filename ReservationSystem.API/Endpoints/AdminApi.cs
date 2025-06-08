using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Domain.Dtos.Product;
using ReservationSystem.Domain.Dtos.Reservation;

namespace ReservationSystem.API.Endpoints;

public static class AdminApi
{
    public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Admin")
            .RequireAuthorization("Admin")
            .WithOpenApi();

        group.MapPost("/products", async (IAdminProductService adminProductService, CreateProductDto dto, CancellationToken ct) =>
        {
            var id = await adminProductService.CreateProductAsync(dto, ct);
            
            return Results.Created($"/admin/products/{id}", id);
        })
        .Produces(StatusCodes.Status201Created);
        
        group.MapPut("/products/{id:guid}", async (IAdminProductService adminProductService, CreateProductDto dto, Guid id, CancellationToken ct) =>
        {
            await adminProductService.UpdateProductAsync(dto, id, ct);
            
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent);
        
        group.MapDelete("/products/{id:guid}", async (IAdminProductService adminProductService, Guid id, CancellationToken ct) =>
        {
            await adminProductService.DeleteProductAsync(id, ct);
            
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent);
        
        group.MapGet("/reservations", async (IReservationService reservationService, [AsParameters] PageQueryFilterDto filterDto, CancellationToken ct) =>
            {
                var result = await reservationService.GetReservationsAsync(filterDto, ct);
            
                return Results.Ok(result);
            })
            .Produces<PaginatedResponseDto<ReservationDto>>(StatusCodes.Status200OK, "application/json");
        
        group.MapGet("/reservations/{id:guid}", async (IReservationService reservationService, Guid id, CancellationToken ct) =>
        {
            var result = await reservationService.GetReservationByIdAsync(id, ct);
        
            return Results.Ok(result);
        })
        .Produces<ReservationDto>(StatusCodes.Status200OK, "application/json");
        
        group.MapPost("/reservations/{id:guid}/confirm", async (IAdminReservationService adminReservationService, Guid id, CancellationToken ct) =>
        {
            await adminReservationService.ConfirmReservationAsync(id, ct);

            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK);
        
        group.MapPost("/reservations/{id:guid}/reject", async (IAdminReservationService adminReservationService, Guid id, CancellationToken ct) =>
        {
            await adminReservationService.RejectReservationAsync(id, ct);

            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK);
        
        group.MapPost("/reservations/{id:guid}/complete", async (IAdminReservationService adminReservationService, Guid id, CancellationToken ct) =>
            {
                await adminReservationService.RejectReservationAsync(id, ct);

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK);
        
        return app;
    }
}