using System.Text.Json.Serialization;
using JobModule.Domain;

namespace JobModule.Models;

public sealed record JobModel
{
    public required Guid Id { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required JobStatus Status { get; init; }

    public string? FailureReason { get; init; }
    public int? FailureCode { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}