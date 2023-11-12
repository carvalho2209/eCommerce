using Application.Abstractions.Messaging;

namespace Application.Products.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price, List<string> Tags) : ICommand;
