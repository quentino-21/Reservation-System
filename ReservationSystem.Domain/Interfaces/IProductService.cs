using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Dtos.Product;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Domain.Interfaces;

public interface IProductService
{
    Task<PaginatedResponseDto<ProductDto>> GetProductsAsync(PageQueryFilterDto filterDto, CancellationToken ct);
    Task<ProductDto> GetProductByIdAsync(Guid id, CancellationToken ct);
}