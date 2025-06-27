using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed record GetCardListByGroupId : IQuery<IReadOnlyList<CardModel>>
{
    public required Guid GroupId { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetCardListByGroupIdHandler(VideoDbContext dbContext) : IQueryHandler<GetCardListByGroupId, IReadOnlyList<CardModel>>
{
    public async Task<Result<IReadOnlyList<CardModel>, MessagingError>> Handle(GetCardListByGroupId request, CancellationToken cancellationToken)
    {
        var cards = await dbContext.Set<Card>()
            .AsNoTracking()
            .Include(card => card.Video)
            .Include(card => card.PreviewVideo)
            .Where(card => card.GroupId == request.GroupId)
            .PagedOrderedDescending(request.Page, card => card.CreatedAt)
            .ToListAsync(cancellationToken);

        return cards.Select(Mapper.MapCardToCardModel).ToList();
    }
}