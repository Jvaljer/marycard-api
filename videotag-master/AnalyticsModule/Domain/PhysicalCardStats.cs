using System.ComponentModel.DataAnnotations;
using AryDotNet.Entity;

namespace AnalyticsModule.Domain;

internal sealed class PhysicalCardStats
{
    [Key, MaxLength(FieldSize.LargeStringLength)]
    public required string CardIdentifier { get; set; }

    public Guid? PhysicalCardId { get; set; }

    public required uint CardVisited { get; set; }
    public required uint TagRedirected { get; set; }
    public Guid? OrderId { get; set; }
}