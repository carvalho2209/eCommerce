using System.Reflection;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    // Add-Migration command for Postgres: 
    // add-migration Name -context PostgresDbContext -outputdir Data\Migrations\Postgres
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
