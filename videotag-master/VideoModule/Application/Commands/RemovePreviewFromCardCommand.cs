using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record RemovePreviewFromCardCommand : ICommand
{
    public required string Identifier { get; init; }
}

internal sealed class DeletePreviewVideoHandler(VideoDbContext dbContext) : ICommandHandler<RemovePreviewFromCardCommand>
{
    public async Task<Result<MessagingError>> Handle(RemovePreviewFromCardCommand request, CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<Card>()
            .FirstOrDefaultAsync(v => v.Identifier == request.Identifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Card not found");
        }

        card.PreviewVideoId = null;
        dbContext.Set<Card>().Update(card);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<MessagingError>.Ok();
    }
}