using AnalyticsModule.Infrastructure;
using AryDotNet.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnalyticsModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    public Task Install(IConfiguration configuration)
    {
        var db = new AnalyticsDbContext(configuration);
        return db.Database.MigrateAsync();
    }
}