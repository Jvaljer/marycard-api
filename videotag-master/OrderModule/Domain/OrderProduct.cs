using AryDotNet.Entity;
using Common.Shopify;

namespace OrderModule.Domain;

internal sealed class OrderProduct : Entity<Guid>
{
    public required ShopifyProductId ShopifyProductId { get; set; }
    public required int Quantity { get; set; }
    public required Guid OrderId { get; set; }

    public Order? Order { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}