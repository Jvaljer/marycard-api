using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class OrderCompany
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("location_id")]
    public long? LocationId { get; set; }
}