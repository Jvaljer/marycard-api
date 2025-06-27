using System.Text.Json.Serialization;
using OrderAPI.Models;

namespace OrderModule.Application.Commands;

public sealed record UpdateOrderRequest
{
    public required string? Note { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required OrderState State { get; init; }
    public required string? CustomerPhone { get; init; }
    public required string? CustomerFirstName { get; init; }
    public required string? CustomerLastName { get; init; }
    public required string? CustomerEmail { get; init; }
    public required string? ContactEmail { get; init; }
}