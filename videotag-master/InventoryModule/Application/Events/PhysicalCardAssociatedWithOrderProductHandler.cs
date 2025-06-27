using AryDotNet.Messaging;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Events;

namespace InventoryModule.Application.Events;

internal sealed class PhysicalCardAssociatedWithOrderProductHandler(InventoryDbContext dbContext)
    : IEventHandler<PhysicalCardAssociatedWithOrderProduct>
{
    public async Task Handle(PhysicalCardAssociatedWithOrderProduct notification, CancellationToken cancellationToken)
    {
        var physicalCard = await dbContext.Set<PhysicalCard>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == notification.PhysicalCardId, cancellationToken);

        if (physicalCard is null || physicalCard.IllustrationId is null)
        {
            return;
        }

        var product = await dbContext.Set<Product>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ShopifyProductId.ProductId == notification.ShopifyProductId.ProductId && p.ShopifyProductId.VariantId == notification.ShopifyProductId.VariantId, cancellationToken);

        if (product is null)
        {
            return;
        }

        var associationAlreadyExists = await dbContext.Set<IllustrationProductAssociation>()
            .AnyAsync(
                association => association.ProductId == product.InternalId &&
                               association.IllustrationId == physicalCard.IllustrationId, cancellationToken);

        if (associationAlreadyExists)
        {
            return;
        }

        await dbContext.Set<IllustrationProductAssociation>().AddAsync(new IllustrationProductAssociation
        {
            ProductId = product.InternalId,
            IllustrationId = physicalCard.IllustrationId.Value
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}