using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public abstract class ShopifyObject
{
    /// <summary>
    /// The object's unique id.
    /// </summary>
    /// <remarks>
    /// Some object ids are longer than the max int32 value. Using long instead.
    /// Marked as nullable due to issues I've run into when trying to create a resource. If Id is present when creating, 
    /// Shopify will try to find that resource. By default it's set to 0 when not null, so the resource won't exist and 
    /// Shopify will return a 404 Not Found. This is most obvious when creating a customer with a <see cref="Address"/> 
    /// and the <see cref="Address"/> Id set to 0.
    /// </remarks>
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("admin_graphql_api_id")]
    public string? AdminGraphQLAPIId { get; set; }
}