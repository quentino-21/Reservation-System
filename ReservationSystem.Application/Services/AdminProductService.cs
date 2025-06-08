using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Dtos.Product;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Infrastructure.Contexts;

namespace ReservationSystem.Application.Services;

public class AdminProductService : IAdminProductService
{
    private readonly ReservationDbContext _context;

    public AdminProductService(ReservationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> CreateProductAsync(CreateProductDto dto, CancellationToken ct)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Duration = dto.Duration,
            Price = dto.Price
        };
        
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);

        return product.Id;
    }

    public async Task UpdateProductAsync(CreateProductDto dto, Guid id, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Duration = dto.Duration;
        product.Price = dto.Price;
        
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }

}