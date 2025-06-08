using ReservationSystem.Domain.Dtos.Product;
using ReservationSystem.Domain.Dtos.Users;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Application.Mappers;

public class ProductMappers
{
    public static ProductDto MapToProductDto(Product product)
    {
        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Duration = product.Duration,
            Price = product.Price,
        };
        return productDto;
    }
}