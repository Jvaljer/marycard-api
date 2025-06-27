using AryDotNet.Entity;
using InventoryModule.Models;

namespace InventoryModule.Domain.Entities;

internal sealed class SyncJob : Entity<Guid>
{
    public required SyncJobStatus Status { get; set; }
}
