using Common.Shopify;

namespace InventoryApi.Models;

public sealed record ProductContentModel
{
    public required string? Name { get; init; }
    public required string? VariantName { get; init; }
    public required string? Description { get; init; }

    public required string? SKU { get; init; }
    public required ShopifyProductId ShopifyProductId { get; init; }
}