using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class CustomerEmailMarketingConsent
{
    /// <summary>
    /// The current email marketing state for the customer.
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    /// <summary>
    /// The marketing subscription opt-in level, as described in the M3AAWG Sender Best Common Practices, that the customer gave when they consented to receive marketing material by email.
    /// </summary>
    [JsonPropertyName("opt_in_level")]
    public string? OptInLevel { get; set; }

    /// <summary>
    /// The date and time when the customer consented to receive marketing material by email. If no date is provided, then the date and time when the consent information was sent is used.
    /// </summary>
    [JsonPropertyName("consent_updated_at")]
    public DateTimeOffset? ConsentUpdatedAt { get; set; }
}