using JobModule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobModule.Infrastructure;

internal sealed class JobDbContext(IConfiguration configuration) : DbContext
{
    private DbSet<Job> Jobs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Job");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
    }
}