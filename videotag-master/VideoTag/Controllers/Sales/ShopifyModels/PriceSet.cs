using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class PriceSet
{
    [JsonPropertyName("shop_money")]
    public Price? ShopMoney { get; set; }

    [JsonPropertyName("presentment_money")]
    public Price? PresentmentMoney { get; set; }
}