using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record UpdateCardPreviewCommand : ICommand
{
    public required string Identifier { get; init; }
    public required Guid? VideoId { get; init; }
}

internal sealed class UpdateCardPreviewHandler(VideoDbContext dbContext) : ICommandHandler<UpdateCardPreviewCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateCardPreviewCommand request, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(video => video.Identifier == request.Identifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        if (request.VideoId is not null)
        {
            var videoExists = await dbContext.Set<PreviewVideo>().AsNoTracking()
                .AnyAsync(v => v.Id == request.VideoId, cancellationToken);

            if (!videoExists)
            {
                return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
            }
        }

        card.PreviewVideoId = request.VideoId;

        dbContext.Set<Card>().Update(card);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}