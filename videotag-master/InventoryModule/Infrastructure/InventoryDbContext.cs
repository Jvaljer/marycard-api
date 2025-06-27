using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventoryModule.Domain.Entities;

namespace InventoryModule.Infrastructure;

internal sealed class InventoryDbContext(IConfiguration configuration) : DbContext
{
    private DbSet<SyncJob> SyncJobs { get; set; } = null!;
    private DbSet<Product> Products { get; set; } = null!;
    private DbSet<PhysicalCard> PhysicalCards { get; set; } = null!;
    private DbSet<Illustration> Illustrations { get; set; } = null!;
    private DbSet<IllustrationProductAssociation> IllustrationProductAssociations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Inventory");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
    }
}