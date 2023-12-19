using MediatR;

namespace Application.Products.UpdateProduct;

public record ProductUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
}
