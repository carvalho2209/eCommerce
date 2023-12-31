﻿using Application.Abstractions.Messaging;
using Domain.Shared;
using MediatR;
using Persistence;

namespace Application.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public UpdateProductCommandHandler(ApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id, cancellationToken);

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

        await _publisher.Publish(
            new ProductUpdatedEvent
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            },
            cancellationToken);

        return Result.Success(product);
    }
}