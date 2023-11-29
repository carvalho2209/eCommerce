namespace Application.Products.CreateProduct;

public record ProductCreatedEvent
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
} 
