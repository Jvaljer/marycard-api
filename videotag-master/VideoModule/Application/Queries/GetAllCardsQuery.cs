using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed class GetAllCardsQuery : IQuery<ICollection<CardModel>>
{
    public required PageQuery Page { get; set; }
}

internal sealed class GetAllVideoHandler(VideoDbContext dbContext) : IQueryHandler<GetAllCardsQuery, ICollection<CardModel>>
{
    public async Task<Result<ICollection<CardModel>, MessagingError>> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
    {
        var videos = await dbContext.Set<Card>().AsNoTracking()
            .Include(card => card.Video)
            .Include(card => card.PreviewVideo)
            .PagedOrderedDescending(request.Page, card => card.CreatedAt)
            .ToListAsync(cancellationToken);

        return videos.Select(Mapper.MapCardToCardModel).ToList();
    }
}