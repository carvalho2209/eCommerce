using Application;
using Application.Behaviors;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddOptions<OutboxSettings>()
    .BindConfiguration("OutBox")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMediatR(ApplicationAssembly.Instance);

builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

var app = builder.Build();

DbMigrationHelpers.EnsureSeedData(app).Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddUserPoints();

app.Run();
