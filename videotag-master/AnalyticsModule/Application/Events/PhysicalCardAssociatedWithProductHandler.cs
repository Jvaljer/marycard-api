using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AryDotNet.Messaging;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Events;
using Serilog;

namespace AnalyticsModule.Application.Events;

internal sealed class PhysicalCardAssociatedWithProductHandler(AnalyticsDbContext dbContext)
    : IEventHandler<PhysicalCardAssociatedWithOrderProduct>
{
    public async Task Handle(PhysicalCardAssociatedWithOrderProduct notification, CancellationToken cancellationToken)
    {
        var cardStats = await dbContext.Set<PhysicalCardStats>()
            .FirstOrDefaultAsync(stats => stats.CardIdentifier == notification.VideoCardId, cancellationToken);

        if (cardStats is null)
        {
            Log.Error("{Module}.{Handler}: Physical card with ID {Id} not found in database.",
                nameof(AnalyticsModule), nameof(PhysicalCardAssociatedWithProductHandler), notification.VideoCardId);
            return;
        }

        cardStats.OrderId = notification.OrderId;
        cardStats.PhysicalCardId = notification.PhysicalCardId;
        dbContext.Set<PhysicalCardStats>().Update(cardStats);


        await dbContext.SaveChangesAsync(cancellationToken);
    }
}