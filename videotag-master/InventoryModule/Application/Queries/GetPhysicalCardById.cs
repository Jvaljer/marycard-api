using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;
using TagClient.Client;
using TagClient.Models.Tag;

namespace InventoryModule.Application.Queries;

public sealed record GetPhysicalCardById : IQuery<PhysicalCardModel>
{
    /// <summary>
    /// Id could either be the physical card id or the tag id.
    /// </summary>
    public required Guid Id { get; init; }
}

internal sealed class GetPhysicalCardByIdHandler(InventoryDbContext dbContext, ITagClient tagClient)
    : IQueryHandler<GetPhysicalCardById, PhysicalCardModel>
{
    public async Task<Result<PhysicalCardModel, MessagingError>> Handle(GetPhysicalCardById request,
        CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<PhysicalCard>().AsNoTracking()
            .Include(p => p.Illustration)
            .FirstOrDefaultAsync(card => card.Id == request.Id || card.TagId == request.Id, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Physical card not found.");
        }

        var tagResult = await tagClient.GetById(card.TagId, cancellationToken);

        return Mapper.MapPhysicalCard(card, tagResult.MapOrElse<TagModel?>(t => t, _ => null));
    }
}