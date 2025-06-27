using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetPhysicalCardListByIllustrationId : IQuery<IReadOnlyList<PhysicalCardModel>>
{
    public required Guid IllustrationId { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetPhysicalCardListByIllustrationIdHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetPhysicalCardListByIllustrationId, IReadOnlyList<PhysicalCardModel>>
{
    public async Task<Result<IReadOnlyList<PhysicalCardModel>, MessagingError>> Handle(GetPhysicalCardListByIllustrationId request, CancellationToken cancellationToken)
    {
        var cards = await dbContext.Set<PhysicalCard>()
            .AsNoTracking()
            .Include(card => card.Illustration)
            .Where(card => card.IllustrationId == request.IllustrationId)
            .PagedOrderedDescending(request.Page, card => card.CreatedAt)
            .ToListAsync(cancellationToken);

        return cards.Select(c => Mapper.MapPhysicalCard(c)).ToList();
    }
}
