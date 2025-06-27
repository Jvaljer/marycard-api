using AryDotNet.Messaging;
using InventoryApi.Models;

namespace InventoryApi.Events;

public sealed record PhysicalCardCreatedEvent : IEvent
{
    public required PhysicalCardModel PhysicalCard { get; init; }
}