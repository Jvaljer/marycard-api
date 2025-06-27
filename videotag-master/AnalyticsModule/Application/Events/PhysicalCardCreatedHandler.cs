using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AryDotNet.Messaging;
using InventoryApi.Events;

namespace AnalyticsModule.Application.Events;

internal sealed class PhysicalCardCreatedHandler(AnalyticsDbContext dbContext) : IEventHandler<PhysicalCardCreatedEvent>
{
    public async Task Handle(PhysicalCardCreatedEvent notification, CancellationToken cancellationToken)
    {
        await dbContext.Set<PhysicalCardStats>()
            .AddAsync(new PhysicalCardStats
            {
                CardIdentifier = notification.PhysicalCard.VideoCardId,
                PhysicalCardId = notification.PhysicalCard.Id,
                CardVisited = 0,
                TagRedirected = 0,
                OrderId = null
            }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}