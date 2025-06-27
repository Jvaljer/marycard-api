namespace VideoTag.Controllers.V2.Order;

public sealed record CreateOrderItemRequest
{
    public required Guid PhysicalCardId { get; init; }
}