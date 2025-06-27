using System.ComponentModel.DataAnnotations;
using AnalyticsModule.Models;
using AryDotNet.Entity;

namespace AnalyticsModule.Domain;

internal sealed class ActivityEvent : Entity<Guid>
{
    [MaxLength(FieldSize.LargeStringLength)]
    public string? CardId { get; set; }
    public required ActivityEventType Type { get; set; }
}