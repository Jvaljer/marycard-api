namespace AnalyticsModule.Models;

public sealed record CardActivityModel
{
    public required ulong VisitCount { get; init; }
    public required DateTime LastVisitAt { get; init; }
}