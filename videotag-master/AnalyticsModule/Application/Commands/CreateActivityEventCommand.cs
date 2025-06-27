using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AnalyticsModule.Models;
using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsModule.Application.Commands;

public sealed record CreateActivityEventCommand : ICommand
{
    public Trusted<string>? CardId { get; init; }
    public required ActivityEventType Type { get; init; }
}

internal sealed class CreateActivityEventHandler(
    AnalyticsDbContext dbContext,
    IRepository<ActivityEvent> activityEventsRepository) : ICommandHandler<CreateActivityEventCommand>
{
    public async Task<Result<MessagingError>> Handle(CreateActivityEventCommand request,
        CancellationToken cancellationToken)
    {
        await activityEventsRepository.AddAsync(new ActivityEvent
        {
            Type = request.Type,
            CardId = request.CardId
        }, cancellationToken);

        if (request.CardId is not null)
        {
            var cardStats = await dbContext.Set<PhysicalCardStats>()
                .FirstOrDefaultAsync(card => card.CardIdentifier == request.CardId, cancellationToken);

            if (cardStats is null)
            {
                await dbContext.Set<PhysicalCardStats>().AddAsync(new PhysicalCardStats
                {
                    CardIdentifier = request.CardId,
                    CardVisited = 1,
                    TagRedirected = 0
                }, cancellationToken);
            }
            else
            {
                cardStats.CardVisited = ((uint)await dbContext.Set<ActivityEvent>()
                    .AsNoTracking()
                    .Where(evt => evt.CardId == request.CardId && evt.Type == ActivityEventType.CardPageVisited)
                    .CountAsync(cancellationToken)) + 1;
                dbContext.Set<PhysicalCardStats>().Update(cardStats);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}