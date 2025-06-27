using AryDotNet.Messaging;
using Common.Shopify;

namespace OrderAPI.Events;

public sealed record PhysicalCardAssociatedWithOrderProduct : IEvent
{
    public required Guid OrderProductId { get; init; }
    public required ShopifyProductId ShopifyProductId { get; init; }
    public required Guid PhysicalCardId { get; init; }
    public required Guid OrderId { get; init; }
    public required string VideoCardId { get; init; }
}