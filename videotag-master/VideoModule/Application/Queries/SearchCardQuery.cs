using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed class SearchCardQuery : IQuery<ICollection<CardModel>>
{
    public required PageQuery Page { get; set; }
    public required string? Identifier { get; set; }
}

internal sealed class SearchCardHandler(VideoDbContext dbContext) : IQueryHandler<SearchCardQuery, ICollection<CardModel>>
{
    public async Task<Result<ICollection<CardModel>, MessagingError>> Handle(SearchCardQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Card>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Identifier))
        {
            query = query.Where(v => v.Identifier.Contains(request.Identifier));
        }

        var videos = await query
            .Include(card => card.Video)
            .Include(card => card.PreviewVideo)
            .PagedOrderedDescending(request.Page, card => card.CreatedAt)
            .ToListAsync(cancellationToken);

        return videos.Select(Mapper.MapCardToCardModel).ToList();
    }
}