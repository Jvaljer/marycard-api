using System.ComponentModel.DataAnnotations.Schema;
using AryDotNet.Entity;

namespace OrderModule.Domain;

internal sealed class OrderItem : Entity<Guid>
{
    [ForeignKey(nameof(Product))]
    public required Guid OrderProductId { get; set; }
    public required Guid PhysicalCardId { get; init; }
    public OrderProduct? Product { get; set; }
}