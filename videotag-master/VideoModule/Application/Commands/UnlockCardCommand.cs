using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed class UnlockCardCommand : ICommand
{
    public required string VideoIdentifier { get; init; }
}

internal sealed class UnlockVideoHandler(VideoDbContext dbContext) : ICommandHandler<UnlockCardCommand>
{
    public async Task<Result<MessagingError>> Handle(UnlockCardCommand request, CancellationToken cancellationToken)
    {
        var video = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(v => v.Identifier == request.VideoIdentifier, cancellationToken);

        if (video is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        video.Locked = false;
        dbContext.Set<Card>().Update(video);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}