using System.Text.Json.Serialization;

namespace AnalyticsModule.Models;

public sealed record ActivityEventModel
{
    public required Guid Id { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ActivityEventType Type { get; init; }
    public string? CardId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}