using System.Text.Json.Serialization;
using AnalyticsModule.Models;

namespace VideoTag.Controllers.V2.Analytics;

public sealed record CreateActivityEventRequest
{
    public string? CardId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ActivityEventType Type { get; init; }
}