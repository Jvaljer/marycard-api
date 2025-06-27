using AryDotNet.Module;
using JobModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    public Task Install(IConfiguration configuration)
    {
        var context = new JobDbContext(configuration);
        return context.Database.MigrateAsync();
    }
}