using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using AryDotNet.Worker;
using Microsoft.EntityFrameworkCore;
using VideoModule.Application.Jobs;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record RemoveVideoFromCardCommand : ICommand
{
    public required string VideoIdentifier { get; init; }
}

internal sealed class DeleteVideoHandler(VideoDbContext dbContext, IBackgroundSender backgroundSender) : ICommandHandler<RemoveVideoFromCardCommand>
{
    public async Task<Result<MessagingError>> Handle(RemoveVideoFromCardCommand request, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(v => v.Identifier == request.VideoIdentifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Card not found.");
        }

        if (card.Locked)
        {
            return new MessagingError(HttpStatusCode.Forbidden, "The card is locked.");
        }

        if (card.VideoId is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "The card does not have a video.");
        }

        var videoIdToDelete = card.VideoId;

        card.VideoId = null;

        dbContext.Set<Card>().Update(card);
        await dbContext.SaveChangesAsync(cancellationToken);

        await backgroundSender.Send(new DeleteVideoJob
        {
            VideoId = videoIdToDelete.Value
        });

        return Result<MessagingError>.Ok();
    }
}