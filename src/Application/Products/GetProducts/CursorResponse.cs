namespace Application.Products.GetProducts;

public sealed record CursorResponse<T>(Guid Cursor, T Data);