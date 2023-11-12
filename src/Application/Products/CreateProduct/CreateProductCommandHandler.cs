using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Persistence;

namespace Application.Products.CreateProduct;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CreateProductCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
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

        return Result.Success();
    }
}
