namespace ReservationSystem.Domain.Dtos.Product;

public record CreateProductDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required TimeSpan Duration { get; set; }
    public Double? Price { get; set; }
}