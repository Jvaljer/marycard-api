using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed class LockCardCommand : ICommand
{
    public required string VideoIdentifier { get; init; }
}

internal sealed class LockVideoHandler(VideoDbContext dbContext) : ICommandHandler<LockCardCommand>
{
    public async Task<Result<MessagingError>> Handle(LockCardCommand request, CancellationToken cancellationToken)
    {
        var video = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(v => v.Identifier == request.VideoIdentifier, cancellationToken);

        if (video is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        if (video.Locked)
        {
            return new MessagingError(HttpStatusCode.UnprocessableEntity, "Video is already locked");
        }

        video.Locked = true;
        video.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.Set<Card>().Update(video);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}