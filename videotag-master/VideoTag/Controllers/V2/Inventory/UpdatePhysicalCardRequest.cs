namespace VideoTag.Controllers.V2.Inventory;

public sealed record UpdatePhysicalCardRequest
{
    public string? Note { get; init; }
    public string? WarehouseCountryCode { get; init; }
    public Guid? IllustrationId { get; init; }
}