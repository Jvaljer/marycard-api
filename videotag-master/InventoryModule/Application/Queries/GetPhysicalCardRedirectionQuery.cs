using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TagClient.Client;
using TagClient.Models.Tag;

namespace InventoryModule.Application.Queries;

public sealed record GetPhysicalCardRedirectionQuery : IQuery<TagRedirectionModel>
{
    public required Guid PhysicalCardId { get; init; }
}

internal sealed class
    GetPhysicalCardRedirectionHandler(InventoryDbContext dbContext, ITagClient tagClient)
    : IQueryHandler<GetPhysicalCardRedirectionQuery, TagRedirectionModel>
{
    public async Task<Result<TagRedirectionModel, MessagingError>> Handle(GetPhysicalCardRedirectionQuery request,
        CancellationToken cancellationToken)
    {
        var physicalCard = await dbContext.Set<PhysicalCard>()
            .FirstOrDefaultAsync(card => card.Id == request.PhysicalCardId, cancellationToken);

        if (physicalCard is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "The physical card does not exist.");
        }

        var tag = await tagClient.GetTagRedirection(physicalCard.TagId, cancellationToken);

        return tag.MapError(Mapper.MapTagError);
    }
}