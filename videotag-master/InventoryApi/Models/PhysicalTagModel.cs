namespace InventoryApi.Models;

public sealed record PhysicalTagModel
{
    public required Guid Id { get; init; }
    public required string PhysicalUid { get; init; }
    public required uint UsageCount { get; init; }
    public required uint LastTagCounter { get; init; }
    public required uint SignatureValidCount { get; init; }
    public required uint SignatureInvalidCount { get; init; }
}