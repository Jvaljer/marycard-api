using ApiVideo.Client;
using AryDotNet.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoModule.Infrastructure;

namespace VideoModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<VideoDbContext>();
        var videoClient = new ApiVideoClient(configuration.GetConnectionString("ApiVideo"));
        serviceCollection.AddSingleton(videoClient);
    }
}