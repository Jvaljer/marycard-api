using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Shopify;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetIllustrationListByShopifyProductId : IQuery<IReadOnlyList<IllustrationModel>>
{
    public required ShopifyProductId ShopifyProductId { get; init; }
}

internal sealed class GetIllustrationListByShopifyProductIdHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetIllustrationListByShopifyProductId, IReadOnlyList<IllustrationModel>>
{
    public async Task<Result<IReadOnlyList<IllustrationModel>, MessagingError>> Handle(
        GetIllustrationListByShopifyProductId request, CancellationToken cancellationToken)
    {
        var illustrations = await dbContext.Set<IllustrationProductAssociation>()
            .AsNoTracking()
            .Include(a => a.Illustration)
            .Where(a => a.Product!.ShopifyProductId.ProductId == request.ShopifyProductId.ProductId &&
                        a.Product.ShopifyProductId.VariantId == request.ShopifyProductId.VariantId)
            .Select(a => a.Illustration!)
            .ToListAsync(cancellationToken);

        return illustrations.Select(Mapper.MapIllustration).ToList();
    }
}