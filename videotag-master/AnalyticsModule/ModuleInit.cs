using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AnalyticsModule.Infrastructure.Repositories;
using AryDotNet.Module;
using AryDotNet.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<AnalyticsDbContext>();
        serviceCollection.AddScoped<IRepository<ActivityEvent>, ActivityEventRepository>();
    }
}