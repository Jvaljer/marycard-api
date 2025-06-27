using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class RefundDuty
{
    [JsonPropertyName("duty_id")]
    public long? DutyId { get; set; }

    [JsonPropertyName("amount_set")]
    public PriceSet? AmountSet { get; set; }
}