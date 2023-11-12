using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Persistence;

namespace Application.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateProductCommandHandler(ApplicationDbContext context) => _context = context;

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);

        if (product == null)
        {
            return Result.Failure(new Error(
                "Product.NotFound",
                $"The product with the Id = {request.Id} was not found."));
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Tags = request.Tags;

        _context.Products.Update(product);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(product);
    }
}