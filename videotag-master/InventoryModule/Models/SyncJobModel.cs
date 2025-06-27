namespace InventoryModule.Models;

public sealed record SyncJobModel
{
    public required SyncJobStatus Status { get; init; }
    public required DateTime CreatedAt { get; init; }
}