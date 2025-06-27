using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Commands;

public sealed record UpdatePhysicalCard : ICommand
{
    public required Guid PhysicalCardId { get; init; }
    public required string? Note { get; init; }
    public required string? CountryCodeWarehouse { get; init; }
    public required Guid? IllustrationId { get; init; }
}

internal sealed class UpdatePhysicalCardHandler(InventoryDbContext dbContext) : ICommandHandler<UpdatePhysicalCard>
{
    public async Task<Result<MessagingError>> Handle(UpdatePhysicalCard request, CancellationToken cancellationToken)
    {
        var physicalCard = await dbContext.Set<PhysicalCard>()
            .FirstOrDefaultAsync(card => card.Id == request.PhysicalCardId, cancellationToken);

        if (physicalCard is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Physical card not found.");
        }

        physicalCard.Note = request.Note;
        physicalCard.CountryCodeWarehouse = request.CountryCodeWarehouse;
        physicalCard.IllustrationId = request.IllustrationId;

        dbContext.Set<PhysicalCard>().Update(physicalCard);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<MessagingError>.Ok();
    }
}