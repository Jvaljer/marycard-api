using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AryDotNet.Messaging;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Events;

namespace AnalyticsModule.Application.Events;

internal sealed class PhysicalCardUnassociatedWithProductHandler(AnalyticsDbContext dbContext) : IEventHandler<PhysicalCardUnassociatedWithOrderProduct>
{
    public async Task Handle(PhysicalCardUnassociatedWithOrderProduct notification, CancellationToken cancellationToken)
    {
        var cardStats = await dbContext.Set<PhysicalCardStats>()
            .FirstOrDefaultAsync(stats => stats.PhysicalCardId == notification.PhysicalCardId, cancellationToken);

        if (cardStats is null)
        {
            return;
        }

        cardStats.OrderId = null;
        dbContext.Set<PhysicalCardStats>().Update(cardStats);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}