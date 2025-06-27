using System.Net;
using ApiVideo.Client;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Jobs;

internal sealed record DeleteVideoJob : IJob
{
    public required Guid VideoId { get; init; }
}

internal sealed class DeleteVideoHandler(VideoDbContext dbContext, ApiVideoClient apiVideoClient) : IJobHandler<DeleteVideoJob>
{
    public async Task<Result<MessagingError>> Handle(DeleteVideoJob request, CancellationToken cancellationToken)
    {
        var video = await dbContext.Set<Video>()
            .FirstOrDefaultAsync(v => v.Id == request.VideoId, cancellationToken);

        if (video is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, $"Video with ID {request.VideoId} not found.");
        }

        var result = await Result<MessagingError>.TryAsync(
            () => apiVideoClient.Videos().deleteAsync(video.ApiVideoId, cancellationToken),
            exception => new MessagingError(HttpStatusCode.InternalServerError, exception.Message));

        if (result.IsError)
        {
            return result;
        }

        dbContext.Set<Video>().Remove(video);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}