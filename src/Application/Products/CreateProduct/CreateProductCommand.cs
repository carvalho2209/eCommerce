using Application.Abstractions.Messaging;

namespace Application.Products.CreateProduct;

public sealed record CreateProductCommand(Guid Id, string Name, decimal Price, List<string> Tags) : ICommand;

