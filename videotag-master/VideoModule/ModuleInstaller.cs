using AryDotNet.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VideoModule.Infrastructure;

namespace VideoModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    public Task Install(IConfiguration configuration)
    {
        var db = new VideoDbContext(configuration);
        return db.Database.MigrateAsync();
    }
}