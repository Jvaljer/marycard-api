using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public record GetAllPreviewVideosQuery : IQuery<ICollection<VideoModel>>
{
    public required PageQuery Page { get; set; }
}

internal sealed class GetAllPreviewVideosHandler(VideoDbContext dbContext) : IQueryHandler<GetAllPreviewVideosQuery, ICollection<VideoModel>>
{
    public async Task<Result<ICollection<VideoModel>, MessagingError>> Handle(GetAllPreviewVideosQuery request, CancellationToken cancellationToken)
    {
        var videos = await dbContext.Set<PreviewVideo>()
            .AsNoTracking()
            .PagedOrderedDescending(request.Page)
            .ToListAsync(cancellationToken);

        return videos.Select(Mapper.MapVideoFromVideoModel).ToList();
    }
}