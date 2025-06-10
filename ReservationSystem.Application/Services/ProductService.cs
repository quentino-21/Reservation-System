using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Mappers;
using ReservationSystem.Domain.Common;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Domain.Dtos.Product;
using ReservationSystem.Infrastructure.Contexts;
using ReservationSystem.Application.Utils;
using ReservationSystem.Domain.Exceptions;

namespace ReservationSystem.Application.Services;

public class ProductService : IProductService
{
    private readonly ReservationDbContext _context;

    public ProductService(ReservationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedResponseDto<ProductDto>> GetProductsAsync(PageQueryFilterDto filterDto, CancellationToken ct)
    {
        var baseQuery = _context.Products.AsQueryable();
        
        var count = baseQuery.Count();

        var items = await baseQuery
            .Select(x => ProductMappers.MapToProductDto(x))
            .Paginate(filterDto.PageNumber, filterDto.PageSize)
            .ToListAsync(ct);
        
        var result = new PaginatedResponseDto<ProductDto>(items, filterDto.PageNumber, filterDto.PageSize, count);
        
        return result;
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        
        var productDto = ProductMappers.MapToProductDto(product);
        
        return productDto;
    }
}