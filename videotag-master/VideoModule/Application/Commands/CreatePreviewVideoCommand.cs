using System.Net;
using ApiVideo.Client;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record CreatePreviewVideoCommand : ICommand<EntityId<Guid>>
{
    public required string VideoTitle { get; init; }
    public required string ApiVideoId { get; init; }
}

internal sealed class CreateVideoHandler(
    VideoDbContext dbContext,
    ApiVideoClient apiVideoClient) : ICommandHandler<CreatePreviewVideoCommand, EntityId<Guid>>
{
    public async Task<Result<EntityId<Guid>, MessagingError>> Handle(CreatePreviewVideoCommand request,
        CancellationToken cancellationToken)
    {
        var apiVideoAlreadyUsed = await dbContext.Set<PreviewVideo>().AsNoTracking()
            .AnyAsync(v => v.ApiVideoId == request.ApiVideoId, cancellationToken);

        if (apiVideoAlreadyUsed)
        {
            return new MessagingError(HttpStatusCode.Conflict, "ApiVideoId already in use.");
        }

        return await Result<ApiVideo.Model.Video, MessagingError>.TryAsync(
            () => apiVideoClient.Videos().getAsync(request.ApiVideoId, cancellationToken),
            (ex) => new MessagingError(HttpStatusCode.NotFound, "Video not found.")
        ).BindAsync(async (video) =>
        {
            var videoId = await dbContext.Set<PreviewVideo>().AddAsync(new PreviewVideo
            {
                Title = request.VideoTitle,
                NormalizedTitle = request.VideoTitle.ToUpperInvariant(),
                ApiVideoId = video.videoid,
                VideoUrl = video.assets.player,
                Playable = false
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<EntityId<Guid>, MessagingError>.Ok(new EntityId<Guid>(videoId.Entity.Id));
        });
    }
}