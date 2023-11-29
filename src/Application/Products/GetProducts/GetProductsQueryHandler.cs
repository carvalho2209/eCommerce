using Application.Abstractions.Messaging;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.GetProducts;

public sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetProductsQueryHandler(ApplicationDbContext context) => _context = context;

    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken)
        => await
            _context
                .Products
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Tags))
                .ToListAsync(cancellationToken);
}