namespace InventoryApi.Models;

public sealed record PhysicalCardModel
{
    public required Guid Id { get; init; }
    public required string VideoCardId { get; init; }
    public required Guid TagId { get; init; }
    public required IllustrationModel? Illustration { get; init; }
    public required PhysicalTagModel? PhysicalTag { get; init; }
    public required string? Note { get; init; }
    public required string? WarehouseCountryCode { get; init; }
    public DateTimeOffset? SoldAt { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}