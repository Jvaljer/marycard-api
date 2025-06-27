using ApiVideo.Client;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed record GetCardByIdQuery : IQuery<CardModel>
{
    public required string Identifier { get; init; }
}

internal sealed class GetVideoByIdHandler(VideoDbContext dbContext, ApiVideoClient videoClient) : IQueryHandler<GetCardByIdQuery, CardModel>
{
    public async Task<Result<CardModel, MessagingError>> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .Include(card => card.Video)
            .Include(card => card.PreviewVideo)
            .Include(card => card.Group)
            .ThenInclude(group => group!.PreviewVideo)
            .FirstOrDefaultAsync(v => v.Identifier == request.Identifier, cancellationToken);

        if (card is null)
        {
            card = new Card
            {
                Identifier = request.Identifier,
                Locked = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await dbContext.Set<Card>().AddAsync(card, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            // Some legacy card might have not been registered in the system.
            //return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        if (card.Video is not null && !card.Video.Playable)
        {
            var apiVideo = await videoClient.Videos().getStatusAsync(card.Video.ApiVideoId, cancellationToken);
            var playable = apiVideo.encoding.playable ?? false;

            if (playable)
            {
                card.Video.Playable = playable;
                dbContext.Set<Video>().Update(card.Video);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        return Mapper.MapCardToCardModel(card);
    }
}