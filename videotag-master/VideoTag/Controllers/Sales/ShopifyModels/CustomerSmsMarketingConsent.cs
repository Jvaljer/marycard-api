using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class CustomerSmsMarketingConsent
{
    /// <summary>
    /// The current SMS marketing state for the customer.
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    /// <summary>
    /// The marketing subscription opt-in level, as described by the M3AAWG best practices guidelines, that the customer gave when they consented to receive marketing material by SMS.
    /// </summary>
    [JsonPropertyName("opt_in_level")]
    public string? OptInLevel { get; set; }

    /// <summary>
    /// The date and time at which the customer consented to receive marketing material by SMS.
    /// </summary>
    [JsonPropertyName("consent_updated_at")]
    public DateTimeOffset? ConsentUpdatedAt { get; set; }

    /// <summary>
    /// The source for whether the customer has consented to receive marketing material by SMS.
    /// </summary>
    [JsonPropertyName("consent_collected_from")]
    public string? ConsentCollectedFrom { get; set; }
}
