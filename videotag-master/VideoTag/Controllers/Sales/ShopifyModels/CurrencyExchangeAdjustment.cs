using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class CurrencyExchangeAdjustment
{
    /// <summary>
    /// The difference between the amounts on the associated transaction and the parent transaction.
    /// </summary>
    [JsonPropertyName("adjustment")]
    public decimal? Adjustment { get; set; }

    /// <summary>
    /// The amount of the parent transaction in the shop currency.
    /// </summary>
    [JsonPropertyName("original_amount")]
    public decimal? OriginalAmount { get; set; }

    /// <summary>
    /// The amount of the associated transaction in the shop currency.
    /// </summary>
    [JsonPropertyName("final_amount")]
    public decimal? FinalAmount { get; set; }

    /// <summary>
    /// The shop currency.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}