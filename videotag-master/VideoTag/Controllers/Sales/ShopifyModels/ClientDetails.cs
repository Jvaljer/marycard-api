using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class ClientDetails
{
    /// <summary>
    /// Shopify does not offer documentation for this field.
    /// </summary>
    [JsonPropertyName("accept_language")]
    public string? AcceptLanguage { get; set; }

    /// <summary>
    /// The browser screen height in pixels, if available.
    /// </summary>
    [JsonPropertyName("browser_height")]
    public int? BrowserHeight { get; set; }

    /// <summary>
    /// The browser IP address.
    /// </summary>
    [JsonPropertyName("browser_ip")]
    public string? BrowserIp { get; set; }

    /// <summary>
    /// The browser screen width in pixels, if available.
    /// </summary>
    [JsonPropertyName("browser_width")]
    public int? BrowserWidth { get; set; }

    /// <summary>
    /// A hash of the session.
    /// </summary>
    [JsonPropertyName("session_hash")]
    public string? SessionHash { get; set; }

    /// <summary>
    /// The browser's user agent string?.
    /// </summary>
    [JsonPropertyName("user_agent")]
    public string? UserAgent { get; set; }
}