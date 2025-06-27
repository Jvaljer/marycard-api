using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record UpdateCardUrlCommand : ICommand
{
    public required string CardIdentifier { get; init; }
    public required string? Url { get; init; }
}

internal sealed class UpdateCardUrlCommandHandler(VideoDbContext dbContext) : ICommandHandler<UpdateCardUrlCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateCardUrlCommand request, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(video => video.Identifier == request.CardIdentifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Video not found.");
        }

        card.Url = request.Url;
        card.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.Set<Card>().Update(card);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}