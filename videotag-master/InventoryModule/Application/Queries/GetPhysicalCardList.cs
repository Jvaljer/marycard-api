using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetPhysicalCardList : IQuery<IReadOnlyList<PhysicalCardModel>>
{
    public required string? CardId { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetPhysicalCardListHandler(InventoryDbContext dbContext) : IQueryHandler<GetPhysicalCardList, IReadOnlyList<PhysicalCardModel>>
{
    public async Task<Result<IReadOnlyList<PhysicalCardModel>, MessagingError>> Handle(GetPhysicalCardList request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<PhysicalCard>().AsNoTracking();

        if (request.CardId is not null)
        {
            query = query.Where(physicalCard => physicalCard.VideoCardId == request.CardId);
        }

        var physicalCards = await query.PagedOrderedDescending(request.Page, physicalCard => physicalCard.CreatedAt)
            .Include(physicalCard => physicalCard.Illustration)
            .ToListAsync(cancellationToken);

        return physicalCards.Select(p => Mapper.MapPhysicalCard(p)).ToList();
    }
}