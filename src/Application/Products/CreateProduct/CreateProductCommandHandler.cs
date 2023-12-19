using Application.Abstractions.Events;
using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using MediatR;
using Persistence;

namespace Application.Products.CreateProduct;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IEventBus _eventBus;
    private readonly IPublisher _publisher;

    public CreateProductCommandHandler(ApplicationDbContext applicationDbContext, IEventBus eventBus, IPublisher publisher)
    {
        _applicationDbContext = applicationDbContext;
        _eventBus = eventBus;
        _publisher = publisher;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Tags = request.Tags,
        };

        _applicationDbContext.Products.Add(product);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var productCreatedEvent = new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        await _publisher.Publish(productCreatedEvent, cancellationToken);
        
        await _eventBus.PublishAsync(productCreatedEvent, cancellationToken);

        return Result.Success();
    }
}
