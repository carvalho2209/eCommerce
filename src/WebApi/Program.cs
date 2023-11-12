using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProducts;
using Application.Products.UpdateProduct;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

DbMigrationHelpers.EnsureSeedData(app).Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.UseHttpsRedirection();

app.Run();
