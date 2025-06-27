using AnalyticsModule.Domain;
using AnalyticsModule.Models;
using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsModule.Application.Queries;

public sealed record GetCardActivityEventsQuery : ICommand<ICollection<ActivityEventModel>>
{
    public required string CardId { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetCardActivityEventsHandler(IRepository<ActivityEvent> activityEventRepository)
    : ICommandHandler<GetCardActivityEventsQuery, ICollection<ActivityEventModel>>
{
    public async Task<Result<ICollection<ActivityEventModel>, MessagingError>> Handle(GetCardActivityEventsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await activityEventRepository.Query().AsNoTracking()
            .Where(evt => evt.CardId == request.CardId)
            .PagedOrderedDescending(request.Page)
            .Select(e => Mapper.MapEventModel(e))
            .ToListAsync(cancellationToken);

        return result;
    }
}