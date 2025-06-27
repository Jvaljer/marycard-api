using AryDotNet.Messaging;
using AryDotNet.Result;
using VideoModule.Infrastructure;
using ApiVideo.Client;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AryDotNet.Worker;
using VideoModule.Application.Jobs;
using VideoModule.Domain.Entities;

namespace VideoModule.Application.Commands;

public sealed class UpdateCardCommand : ICommand
{
    public required string ApiVideoId { get; init; }
    public required string VideoTitle { get; init; }
    public required string Identifier { get; init; }
}

internal sealed class UpdateVideoHandler(
    VideoDbContext dbContext,
    ApiVideoClient videoClient, IBackgroundSender backgroundSender) : ICommandHandler<UpdateCardCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateCardCommand cmd, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(video => video.Identifier == cmd.Identifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        if (card.Locked)
        {
            return new MessagingError(HttpStatusCode.UnprocessableEntity, "Video is already locked");
        }

        var videoUsed = await dbContext.Set<Video>().AsNoTracking()
            .AnyAsync(v => v.ApiVideoId == cmd.ApiVideoId, cancellationToken);

        if (videoUsed)
        {
            return new MessagingError(HttpStatusCode.Conflict, "Video already in use.");
        }

        return await Result<ApiVideo.Model.Video, MessagingError>.TryAsync(
                () => videoClient.Videos().getAsync(cmd.ApiVideoId, cancellationToken),
                e => new MessagingError(HttpStatusCode.NotFound, e.Message))
            .BindAsync(async v =>
            {
                var r = await videoClient.Videos().updateAsync(v.videoid, new ApiVideo.Model.VideoUpdatePayload
                {
                    title = cmd.VideoTitle
                }, cancellationToken);

                return r is null
                    ? Result<ApiVideo.Model.Video, MessagingError>.Failure(
                        new MessagingError(HttpStatusCode.InternalServerError, "Error"))
                    : Result<ApiVideo.Model.Video, MessagingError>.Ok(r);
            })
            .BindAsync(async v =>
            {
                var oldVideoId = card.VideoId;
                var createdVideoId = await dbContext.Set<Video>().AddAsync(new Video
                {
                    ApiVideoId = v.videoid,
                    Title = cmd.VideoTitle,
                    NormalizedTitle = cmd.VideoTitle.ToUpperInvariant(),
                    VideoUrl = v.assets.player,
                    Playable = false
                }, cancellationToken);

                card.VideoId = createdVideoId.Entity.Id;

                dbContext.Set<Card>().Update(card);
                await dbContext.SaveChangesAsync(cancellationToken);

                if (oldVideoId is not null)
                {
                    await backgroundSender.Send(new DeleteVideoJob
                    {
                        VideoId = oldVideoId.Value
                    });
                }

                return Result<MessagingError>.Ok();
            });
    }
}