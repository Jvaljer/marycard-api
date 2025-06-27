using AryDotNet.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderModule.BackgroundService;
using OrderModule.Infrastructure;

namespace OrderModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<OrderDbContext>();
        serviceCollection.AddHostedService<OrderSyncService>();
    }
}