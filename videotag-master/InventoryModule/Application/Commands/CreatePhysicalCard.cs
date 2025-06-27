using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Models;
using InventoryApi.Events;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Commands;

public sealed record CreatePhysicalCard : ICommand<EntityId<Guid>>
{
    public required Guid? IllustrationId { get; init; }
    public required string VideoCardId { get; init; }
    public required Guid TagId { get; init; }
    public required string? CountryCodeWarehouse { get; init; }
}

internal sealed class CreatePhysicalCardHandler(InventoryDbContext dbContext, IPublisher publisher)
    : ICommandHandler<CreatePhysicalCard, EntityId<Guid>>
{
    public async Task<Result<EntityId<Guid>, MessagingError>> Handle(CreatePhysicalCard request,
        CancellationToken cancellationToken)
    {
        if (request.IllustrationId is not null)
        {
            var illustrationExists = await dbContext.Set<Illustration>()
                .AnyAsync(i => i.Id == request.IllustrationId, cancellationToken);

            if (!illustrationExists)
            {
                return new MessagingError(HttpStatusCode.NotFound, "Illustration not found.");
            }
        }

        var cardAlreadyExists = await dbContext.Set<PhysicalCard>()
            .AnyAsync(c => c.VideoCardId == request.VideoCardId, cancellationToken);

        if (cardAlreadyExists)
        {
            return new MessagingError(HttpStatusCode.Conflict, "Card already exists.");
        }

        var card = new PhysicalCard
        {
            VideoCardId = request.VideoCardId,
            CreatedAt = DateTimeOffset.UtcNow,
            IllustrationId = request.IllustrationId,
            CountryCodeWarehouse = request.CountryCodeWarehouse,
            TagId = request.TagId
        };

        var entityEntry = await dbContext.Set<PhysicalCard>().AddAsync(card, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await publisher.Publish(new PhysicalCardCreatedEvent
        {
            PhysicalCard = Mapper.MapPhysicalCard(card)
        }, cancellationToken);

        return new EntityId<Guid>(entityEntry.Entity.Id);
    }
}