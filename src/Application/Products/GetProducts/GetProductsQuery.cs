using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed record GetProductsQuery : IQuery<List<ProductResponse>>;

public sealed record GetProductsCursorQuery(Guid Cursor, int PageSize) : IQuery<CursorResponse<List<ProductResponse>>>;

