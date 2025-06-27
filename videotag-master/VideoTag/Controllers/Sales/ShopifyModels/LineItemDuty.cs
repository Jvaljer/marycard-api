using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class LineItemDuty
{
    [JsonPropertyName("harmonized_system_code")]
    public string? HarmonizedSystemCode { get; set; }

    [JsonPropertyName("country_code_of_origin")]
    public string? CountryCodeOfOrigin { get; set; }

    [JsonPropertyName("price_set")]
    public PriceSet? PriceSet { get; set; }

    [JsonPropertyName("tax_lines")]
    public IEnumerable<TaxLine>? TaxLines { get; set; }
}