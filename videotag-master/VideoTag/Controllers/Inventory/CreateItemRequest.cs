namespace VideoTag.Controllers.Inventory;

public sealed record CreateItemRequest
{
    public required string CardId { get; init; }
    public required Guid? TagId { get; init; }
}