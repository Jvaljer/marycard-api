namespace OrderAPI.Models;

public sealed record OrderItemModel
{
    public required Guid PhysicalCardId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}