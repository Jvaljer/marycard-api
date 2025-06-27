using AryDotNet.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderModule.Infrastructure;

namespace OrderModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    public Task Install(IConfiguration configuration)
    {
        var db = new OrderDbContext(configuration);
        return db.Database.MigrateAsync();
    }
}