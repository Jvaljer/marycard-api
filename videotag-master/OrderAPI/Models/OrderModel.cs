using System.Text.Json.Serialization;

namespace OrderAPI.Models;

public sealed record OrderModel
{
    public required Guid Id { get; init; }
    public required ulong ShopifyOrderId { get; init; }
    public required ulong ShopifyCustomerId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required OrderState State { get; init; }

    public string? CustomerEmail { get; init; }

    public string? ContactEmail { get; init; }
    public string? CustomerPhone { get; init; }
    public string? CustomerFirstName { get; init; }
    public string? CustomerLastName { get; init; }
    public string? Note { get; init; }

    public DateTime ShopifyCreatedAt { get; init; }
    public DateTime ShopifyUpdatedAt { get; init; }
    public required ICollection<OrderProductModel> Products { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}