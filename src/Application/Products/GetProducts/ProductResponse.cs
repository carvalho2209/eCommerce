namespace Application.Products.GetProducts;

public sealed record ProductResponse(Guid Id, string? Name, decimal Price, List<string> Tags);
