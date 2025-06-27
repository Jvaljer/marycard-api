using AuthModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthModule.Infrastructure;

internal sealed class AuthDbContext(IConfiguration configuration) : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions => sqlOptions.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Auth");
        modelBuilder.ApplyConfigurationsFromAssembly(Reference.Assembly);
        base.OnModelCreating(modelBuilder);
    }

}