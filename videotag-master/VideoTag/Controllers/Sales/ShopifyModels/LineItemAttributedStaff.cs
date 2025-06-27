using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class LineItemAttributedStaff
{
    /// <summary>
    /// The GraphQL id of the staff member
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The quantity of the line item attributed to the staff member.
    /// </summary>
    [JsonPropertyName("quantity")]
    public long? Quantity { get; set; }
}