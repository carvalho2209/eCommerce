using Application.Products.CreateProduct;
using Application.Products.GetProducts;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Configurations;

public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (IMediator sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());

                return Results.Ok(result);
            })
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetProducts")
            .WithTags("Products");

        app.MapPost("api/products",
                async ([FromBody] CreateProductCommand request,
                    IMediator sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request, cancellationToken);

                    return Results.Ok(result);
                })
            .Produces<CreateProductCommand>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("PostProducts")
            .WithTags("Products");
    }
}
