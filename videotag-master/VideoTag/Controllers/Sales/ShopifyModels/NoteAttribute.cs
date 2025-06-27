using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class NoteAttribute
{
    /// <summary>
    /// The name of the note attribute.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The value of the note attribute.
    /// </summary>
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}