using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class FulfillmentOriginAddress
{
    /// <summary>
    /// The street address of the fulfillment location.
    /// </summary>
    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    /// <summary>
    /// The second line of the address. Typically the number of the apartment, suite, or unit.
    /// </summary>
    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    /// <summary>
    /// The city of the fulfillment location.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    /// (Required) The two-letter country code (ISO 3166-1 alpha-2 format) of the fulfillment location.
    /// </summary>
    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    /// <summary>
    /// The province of the fulfillment location.
    /// </summary>
    [JsonPropertyName("province_code")]
    public string? ProvinceCode { get; set; }

    /// <summary>
    /// The zip code of the fulfillment location.
    /// </summary>
    [JsonPropertyName("zip")]
    public string? Zip { get; set; }
}