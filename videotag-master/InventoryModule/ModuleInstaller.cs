using AryDotNet.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventoryModule.Infrastructure;

namespace InventoryModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    public Task Install(IConfiguration configuration)
    {
        var db = new InventoryDbContext(configuration);
        return db.Database.MigrateAsync();
    }
}