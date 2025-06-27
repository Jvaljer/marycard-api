using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Shopify;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetProductByIdQuery : IQuery<ProductModel>
{
    public required ShopifyProductId ShopifyProductId { get; init; }
}

internal sealed class GetProductByIdHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, ProductModel>
{
    public async Task<Result<ProductModel, MessagingError>> Handle(GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Set<Product>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.ShopifyProductId.ProductId == request.ShopifyProductId.ProductId &&
                     x.ShopifyProductId.VariantId == request.ShopifyProductId.VariantId, cancellationToken);
        if (product is null)
        {
            return new MessagingError(System.Net.HttpStatusCode.NotFound, "Product not found");
        }

        return Mapper.ProductToProductModel(product);
    }
}