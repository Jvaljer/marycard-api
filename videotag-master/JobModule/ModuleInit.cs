using AryDotNet.Module;
using AryDotNet.Repository;
using JobModule.Domain;
using JobModule.Infrastructure;
using JobModule.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<JobDbContext>();
        serviceCollection.AddScoped<IRepository<Job>, JobRepository>();
    }
}