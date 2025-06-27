using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class RefundDutyType
{
    [JsonPropertyName("duty_id")]
    public long? DutyId { get; set; }

    [JsonPropertyName("refund_type")]
    public string? RefundType { get; set; }
}