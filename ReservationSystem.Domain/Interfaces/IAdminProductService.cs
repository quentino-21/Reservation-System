using ReservationSystem.Domain.Dtos.Product;

namespace ReservationSystem.Domain.Interfaces;

public interface IAdminProductService
{
    Task<Guid> CreateProductAsync(CreateProductDto dto, CancellationToken ct);
    Task UpdateProductAsync(CreateProductDto dto, Guid id, CancellationToken ct);
    Task DeleteProductAsync(Guid id, CancellationToken ct);
}