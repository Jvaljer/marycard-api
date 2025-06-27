namespace AnalyticsModule.Models;

public sealed record OrderStatsModel
{
    public required uint TotalCardVisited { get; init; }
    public required uint TotalTagRedirected { get; init; }
}