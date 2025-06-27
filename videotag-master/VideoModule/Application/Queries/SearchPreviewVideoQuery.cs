using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public record SearchPreviewVideoQuery : IQuery<ICollection<VideoModel>>
{
    public required string Query { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class SearchPreviewVideoHandler(VideoDbContext dbContext) : IQueryHandler<SearchPreviewVideoQuery, ICollection<VideoModel>>
{
    public async Task<Result<ICollection<VideoModel>, MessagingError>> Handle(SearchPreviewVideoQuery request, CancellationToken cancellationToken)
    {
        var lowered = request.Query.ToLowerInvariant();
        var normalized = request.Query.ToUpperInvariant();
        var videos = await dbContext.Set<PreviewVideo>()
            .AsNoTracking()
            .Where(video => video.NormalizedTitle.Contains(normalized) || video.ApiVideoId.Contains(lowered))
            .PagedOrderedDescending(request.Page)
            .ToListAsync(cancellationToken);

        return videos.Select(Mapper.MapVideoFromVideoModel).ToList();
    }
}