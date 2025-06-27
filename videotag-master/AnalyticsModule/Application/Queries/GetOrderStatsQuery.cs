using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AnalyticsModule.Models;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsModule.Application.Queries;

public sealed record GetOrderStatsQuery : IQuery<OrderStatsModel>
{
    public required Guid OrderId { get; init; }
}

internal sealed class GetOrderStatsQueryHandler(AnalyticsDbContext dbContext) : IQueryHandler<GetOrderStatsQuery, OrderStatsModel>
{
    public async Task<Result<OrderStatsModel, MessagingError>> Handle(GetOrderStatsQuery request, CancellationToken cancellationToken)
    {
        var c = await dbContext.Set<PhysicalCardStats>()
            .AsNoTracking()
            .Where(card => card.OrderId == request.OrderId)
            .SumAsync(s => s.CardVisited, cancellationToken);

        var t = await dbContext.Set<PhysicalCardStats>()
            .AsNoTracking()
            .Where(card => card.OrderId == request.OrderId)
            .SumAsync(s => s.TagRedirected, cancellationToken);

        return new OrderStatsModel
        {
            TotalCardVisited = (uint)c,
            TotalTagRedirected = (uint)t
        };
    }
}