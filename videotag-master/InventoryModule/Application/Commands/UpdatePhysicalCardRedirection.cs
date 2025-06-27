using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TagClient.Client;

namespace InventoryModule.Application.Commands;

public sealed record UpdatePhysicalCardRedirection : ICommand
{
    public required Guid PhysicalCardId { get; init; }
    public required string? Url { get; init; }
}

internal sealed class UpdatePhysicalCardRedirectionHandler(InventoryDbContext dbContext, ITagClient tagClient)
    : ICommandHandler<UpdatePhysicalCardRedirection>
{
    public async Task<Result<MessagingError>> Handle(UpdatePhysicalCardRedirection request,
        CancellationToken cancellationToken)
    {
        var card = await dbContext.Set<PhysicalCard>()
            .FirstOrDefaultAsync(c => c.Id == request.PhysicalCardId, cancellationToken);

        if (card is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Physical card not found.");
        }

        if (request.Url is null)
        {
            var result = await tagClient.DeleteTagRedirection(card.TagId, cancellationToken);

            if (result.IsError)
            {
                return result.MapError(Mapper.MapTagError);
            }
        }
        else
        {
            var result = await tagClient.CreateOrUpdateTagRedirection(card.TagId, request.Url, cancellationToken);

            if (result.IsError)
            {
                return result.MapError(Mapper.MapTagError);
            }
        }

        return Result<MessagingError>.Ok();
    }
}