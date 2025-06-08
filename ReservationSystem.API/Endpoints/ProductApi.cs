using Microsoft.AspNetCore.Mvc;
using ReservationSystem.API.Extensions;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Domain.Dtos.Product;

namespace ReservationSystem.API.Endpoints;

public static class ProductApi
{
    public static IEndpointRouteBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .AllowAnonymous()
            .WithOpenApi();

        group.MapGet("", async (IProductService productService, [AsParameters] PageQueryFilterDto filterDto, CancellationToken ct) =>
        {
            var result = await productService.GetProductsAsync(filterDto, ct);

            return Results.Ok(result);
        })
        .WithRequestValidation<PageQueryFilterDto>()
        .Produces<PaginatedResponseDto<ProductDto>>(StatusCodes.Status200OK, "application/json");
        
        group.MapGet("/{id:guid}", async (IProductService productService, Guid id, CancellationToken ct) =>
        {
            var result = await productService.GetProductByIdAsync(id, ct);
            
            return Results.Ok(result);
        })
        .Produces<ProductDto>(StatusCodes.Status200OK, "application/json");
        
        return app;
    }
}