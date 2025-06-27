using AnalyticsModule.Domain;
using AnalyticsModule.Models;
using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsModule.Application.Queries;

public sealed record GetCardActivityQuery : IQuery<CardActivityModel>
{
    public required string CardId { get; init; }
}

internal sealed class GetCardActivityHandler(IRepository<ActivityEvent> activityRepository)
    : IQueryHandler<GetCardActivityQuery, CardActivityModel>
{
    public async Task<Result<CardActivityModel, MessagingError>> Handle(GetCardActivityQuery request,
        CancellationToken cancellationToken)
    {
        var visitCount = await activityRepository
            .Query().AsNoTracking()
            .CountAsync(x => x.CardId == request.CardId && x.Type == ActivityEventType.CardPageVisited,
                cancellationToken);

        var lastVisit = await activityRepository
            .Query().AsNoTracking()
            .Where(x => x.CardId == request.CardId && x.Type == ActivityEventType.CardPageVisited)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return new CardActivityModel
        {
            VisitCount = (ulong)visitCount,
            LastVisitAt = lastVisit
        };
    }
}