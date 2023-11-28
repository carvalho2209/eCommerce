using Application.Abstractions.Messaging;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.GetProducts;

internal sealed class GetProductsCursorQueryHandler :
    IQueryHandler<GetProductsCursorQuery, CursorResponse<List<ProductResponse>>>
{
    private readonly ApplicationDbContext _context;

    public GetProductsCursorQueryHandler(ApplicationDbContext context) => _context = context;

    public async Task<Result<CursorResponse<List<ProductResponse>>>> Handle(GetProductsCursorQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ProductResponse> products = await _context
            .Products
            .Where(p => p.Id >= request.Cursor)
            .Take(request.PageSize + 1)
            .OrderBy(p => p.Id)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Price,
                p.Tags))
            .ToListAsync(cancellationToken);

        Guid cursor = products[^1].Id;

        List<ProductResponse> productResponses = products.Take(request.PageSize).ToList();

        return new CursorResponse<List<ProductResponse>>(cursor, productResponses);
    }
}
