using Common.Shopify;

namespace InventoryApi.Models;

public sealed record ProductModel
{
    public required ShopifyProductId ShopifyProductId { get; init; }
    public required string Name { get; init; }
    public required string VariantName { get; init; }
    public required string Description { get; init; }

    public required string? SKU { get; init; }

    public required bool Deleted { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
}
