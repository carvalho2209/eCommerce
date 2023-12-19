using Application.Abstractions.Messaging;
using Domain.Shared;
using MediatR;
using Persistence;

namespace Application.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public DeleteProductCommandHandler(ApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);

        if (product is not null)
            _context.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new ProductDeletedEvent { Id = request.Id }, cancellationToken);

        return Result.Success();
    }
}