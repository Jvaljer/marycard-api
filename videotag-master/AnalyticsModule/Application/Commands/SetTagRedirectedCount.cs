using System.Net;
using AnalyticsModule.Domain;
using AnalyticsModule.Infrastructure;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsModule.Application.Commands;

public sealed record SetTagRedirectedCount : ICommand
{
    public required Guid PhysicalCardId { get; init; }
    public required uint TagRedirectedCount { get; init; }
}

internal sealed class SetTagRedirectedCountHandler(AnalyticsDbContext dbContext)
    : ICommandHandler<SetTagRedirectedCount>
{
    public async Task<Result<MessagingError>> Handle(SetTagRedirectedCount request, CancellationToken cancellationToken)
    {
        var physicalCard = await dbContext.Set<PhysicalCardStats>()
            .FirstOrDefaultAsync(card => card.PhysicalCardId == request.PhysicalCardId, cancellationToken);

        if (physicalCard is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Physical card not found.");
        }

        physicalCard.TagRedirected = request.TagRedirectedCount;
        dbContext.Set<PhysicalCardStats>().Update(physicalCard);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<MessagingError>.Ok();
    }
}