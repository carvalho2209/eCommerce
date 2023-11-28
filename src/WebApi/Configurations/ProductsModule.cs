using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProducts;
using Application.Products.UpdateProduct;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Configurations;

public static class ProductsModule
{
    public static void AddUserPoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (int page, int pageSize, IMediator sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(page, pageSize));

                return Results.Ok(result);
            })
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
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

        app.MapPut("api/products/{productId}",
                async (
                    Guid productId,
                    [FromBody] UpdateProductCommand request,
                    IMediator sender) =>
                {
                    var command = new UpdateProductCommand(productId, request.Name, request.Price, request.Tags);

                    Result result = await sender.Send(command);

                    return result.IsFailure
                        ? Results.NotFound(result.Error)
                        : Results.Ok(result);
                })
            .Produces<UpdateProductCommand>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("PutProducts")
            .WithTags("Products");

        app.MapDelete("/products/{productId}", async (Guid productId, IMediator sender) =>
            {
                await sender.Send(new DeleteProductCommand(productId));

                return Results.NoContent();
            })
            .Produces<DeleteProductCommand>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("DeleteProducts")
            .WithTags("Products");
    }
}