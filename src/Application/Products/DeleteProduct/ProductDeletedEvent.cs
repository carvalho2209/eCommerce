using MediatR;

namespace Application.Products.DeleteProduct;

public record ProductDeletedEvent : INotification
{
    public Guid Id { get; set; }
}
