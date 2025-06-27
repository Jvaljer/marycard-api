using AryDotNet.Module;
using AryDotNet.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Infrastructure.Repositories;
using InventoryModule.BackgroundServices;

namespace InventoryModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<InventoryDbContext>();
        serviceCollection.AddScoped<IRepository<SyncJob>, SyncJobRepository>();

        serviceCollection.AddHostedService<ProductSyncService>();
    }
}