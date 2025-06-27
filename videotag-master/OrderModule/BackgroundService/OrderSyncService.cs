using AryDotNet.Worker;
using Microsoft.Extensions.DependencyInjection;
using OrderModule.Application.Jobs;
using Serilog;

namespace OrderModule.BackgroundService;

internal sealed class OrderSyncService(IServiceScopeFactory serviceScopeFactory)
    : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var sender = scope.ServiceProvider.GetRequiredService<IBackgroundSender>();
            var result = await sender.Send(new SyncOrdersJob());
            Log.Information("{Service}: Sync job started {JobId}", nameof(OrderSyncService), result);
            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }
}