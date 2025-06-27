using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class TaxLine
{
    /// <summary>
    /// Whether the channel that submitted the tax line is responsible for remitting it.
    /// </summary>
    [JsonPropertyName("channelLiable")]
    public bool? ChannelLiable { get; set; }

    /// <summary>
    /// The amount of tax to be charged.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// The rate of tax to be applied.
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal? Rate { get; set; }

    /// <summary>
    /// The proportion of the line item price represented by the tax, expressed as a percentage.
    /// </summary>
    [JsonPropertyName("ratePercentage")]
    public decimal? RatePercentage { get; set; }

    /// <summary>
    /// The origin of the tax.
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// The name of the tax.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The amount added to the order for this tax in shop and presentment currencies.
    /// </summary>
    [JsonPropertyName("price_set")]
    public PriceSet? PriceSet { get; set; }
}