using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderModule.Domain;

namespace OrderModule.Infrastructure;

internal sealed class OrderDbContext(IConfiguration configuration) : DbContext
{
    private DbSet<Order> Orders { get; set; } = null!;
    private DbSet<OrderProduct> OrderProducts { get; set; } = null!;
    private DbSet<OrderItem> OrderItems { get; set; } = null!;
    private DbSet<Sync> Syncs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"),
            sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Order");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
    }
}