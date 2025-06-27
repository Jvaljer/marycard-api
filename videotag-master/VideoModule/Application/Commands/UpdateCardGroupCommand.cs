using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record UpdateCardGroupCommand : ICommand
{
    public required string Identifier { get; init; }
    public required Guid? GroupId { get; init; }
}

internal sealed class UpdateCardGroupHandler(VideoDbContext videoDbContext) : ICommandHandler<UpdateCardGroupCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateCardGroupCommand request, CancellationToken cancellationToken)
    {
        var card = await videoDbContext.Set<Card>()
            .FirstOrDefaultAsync(v => v.Identifier == request.Identifier, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Card not found");
        }

        card.GroupId = request.GroupId;
        videoDbContext.Set<Card>().Update(card);
        await videoDbContext.SaveChangesAsync(cancellationToken);
        return Result<MessagingError>.Ok();
    }
}