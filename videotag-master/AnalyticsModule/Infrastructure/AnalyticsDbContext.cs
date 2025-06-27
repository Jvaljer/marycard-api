using AnalyticsModule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnalyticsModule.Infrastructure;

internal sealed class AnalyticsDbContext(IConfiguration configuration) : DbContext
{
    private DbSet<ActivityEvent> ActivityEvents { get; set; } = null!;
    private DbSet<PhysicalCardStats> PhysicalCardStats { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Analytics");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
    }
}