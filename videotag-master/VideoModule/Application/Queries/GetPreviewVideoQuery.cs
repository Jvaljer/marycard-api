using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public record GetPreviewVideoQuery : IQuery<VideoModel>
{
    public required Guid VideoId { get; init; }
}

internal sealed class GetPreviewVideoHandler(VideoDbContext dbContext) : IQueryHandler<GetPreviewVideoQuery, VideoModel>
{
    public async Task<Result<VideoModel, MessagingError>> Handle(GetPreviewVideoQuery request, CancellationToken cancellationToken)
    {
        var video = await dbContext.Set<PreviewVideo>()
            .AsNoTracking()
            .FirstOrDefaultAsync(video => video.Id == request.VideoId, cancellationToken);

        if (video is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        return Mapper.MapVideoFromVideoModel(video);
    }
}