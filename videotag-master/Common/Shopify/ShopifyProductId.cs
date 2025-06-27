using Microsoft.EntityFrameworkCore;

namespace Common.Shopify;

[Owned]
public sealed record ShopifyProductId
{
    public required ulong ProductId { get; set; }
    public required ulong VariantId { get; set; }
}