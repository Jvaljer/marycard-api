using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VideoModule.Domain.Entities;

namespace VideoModule.Infrastructure;

internal sealed class VideoDbContext(IConfiguration configuration) : DbContext
{
    private DbSet<Card> Cards { get; set; } = null!;
    private DbSet<Video> Videos { get; set; } = null!;
    private DbSet<PreviewVideo> PreviewVideos { get; set; } = null!;
    private DbSet<Group> Groups { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Video");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
    }

}