using Application.Abstractions.Messaging;
using Domain.Shared;
using Persistence;

namespace Application.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductCommandHandler(ApplicationDbContext context) => _context = context;

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);

        if (product is not null)
            _context.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}