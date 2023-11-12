using System.ComponentModel.DataAnnotations;

namespace Domain.Products;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public required List<string> Tags { get; set; } 
}