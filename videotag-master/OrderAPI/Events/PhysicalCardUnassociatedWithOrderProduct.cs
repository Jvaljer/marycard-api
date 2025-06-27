using AryDotNet.Messaging;

namespace OrderAPI.Events;

public sealed record PhysicalCardUnassociatedWithOrderProduct : IEvent
{
    public required Guid PhysicalCardId { get; init; }
    public required Guid OrderId { get; init; }
    public required Guid ProductId { get; init; }
}