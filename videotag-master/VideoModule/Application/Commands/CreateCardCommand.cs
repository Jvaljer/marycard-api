using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record CreateCardCommand : ICommand<EntityId<string>>
{
    public required string Identifier { get; init; }
    public required string? Url { get; init; }
}

internal sealed class CreateCardHandler(VideoDbContext dbContext) : ICommandHandler<CreateCardCommand, EntityId<string>>
{
    public async Task<Result<EntityId<string>, MessagingError>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.Set<Card>()
            .AsNoTracking()
            .AnyAsync(v => v.Identifier == request.Identifier, cancellationToken);

        if (alreadyExists)
        {
            return new MessagingError(HttpStatusCode.Conflict, "Card already exists.");
        }

        var card = new Card
        {
            Identifier = request.Identifier,
            Locked = false,
            Url = request.Url,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await dbContext.Set<Card>().AddAsync(card, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new EntityId<string>(request.Identifier);
    }
}