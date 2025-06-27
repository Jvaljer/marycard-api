using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class LineItemProperty
{
    /// <summary>
    /// The name of the note attribute.
    /// </summary>
    [JsonPropertyName("name")]
    public object? Name { get; set; }

    /// <summary>
    /// The value of the note attribute.
    /// </summary>
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}