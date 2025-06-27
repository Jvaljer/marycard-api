using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record CheckCardIdentifierCommand : ICommand<Trusted<string>>
{
    public required string Identifier { get; init; }
}

internal sealed class CheckCardIdentifierHandler(VideoDbContext dbContext)
    : ICommandHandler<CheckCardIdentifierCommand, Trusted<string>>
{
    public async Task<Result<Trusted<string>, MessagingError>> Handle(CheckCardIdentifierCommand request,
        CancellationToken cancellationToken)
    {
        var cardExists = await dbContext.Set<Card>().AsNoTracking()
            .AnyAsync(c => c.Identifier == request.Identifier, cancellationToken);

        if (!cardExists)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Card not found");
        }

        return Trusted<string>.From(request.Identifier);
    }
}