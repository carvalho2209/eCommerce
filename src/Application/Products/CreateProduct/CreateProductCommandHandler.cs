using Application.Abstractions.Events;
using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Persistence;

namespace Application.Products.CreateProduct;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IEventBus _eventBus;

    public CreateProductCommandHandler(ApplicationDbContext applicationDbContext, IEventBus eventBus)
    {
        _applicationDbContext = applicationDbContext;
        _eventBus = eventBus;
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

        await _eventBus.PublishAsync(
            new ProductCreatedEvent
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
            }, cancellationToken);

        return Result.Success();
    }
}
