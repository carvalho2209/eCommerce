using MediatR;

namespace Application.Products.CreateProduct;

public record ProductCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
} 
