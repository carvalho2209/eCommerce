using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed class GetProductsQuery : IQuery<List<ProductResponse>>;
