using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed record GetProductsQuery(int Page, int PageSize) : IQuery<List<ProductResponse>>;

public sealed record GetProductsCursorQuery(Guid Cursor, int PageSize) : IQuery<CursorResponse<List<ProductResponse>>>;

