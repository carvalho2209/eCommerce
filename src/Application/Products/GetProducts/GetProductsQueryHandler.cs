using Application.Abstractions.Messaging;
using Application.Caching;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.GetProducts;

public sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public GetProductsQueryHandler(ApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken) =>
        await _cacheService.GetAsync(
            "products",
            async () =>
            {
                IReadOnlyList<ProductResponse> products = await _context
                    .Products
                    .Select(p => new ProductResponse(
                        p.Id,
                        p.Name,
                        p.Price,
                        p.Tags))
                    .ToListAsync(cancellationToken);

                return products.ToList();
            },
            cancellationToken);
}