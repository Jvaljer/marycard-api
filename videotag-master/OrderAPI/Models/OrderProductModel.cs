using Common.Shopify;

namespace OrderAPI.Models;

public sealed class OrderProductModel
{
    public required Guid Id { get; init; }
    public required ShopifyProductId ShopifyProductId { get; init; }
    public required int Quantity { get; init; }
    public required ICollection<OrderItemModel> Items { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}