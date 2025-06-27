using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class RefundLineItem
{
    /// <summary>
    /// The single <see cref="ShopifySharp.LineItem"/> being returned.
    /// </summary>
    [JsonPropertyName("line_item")]
    public LineItem? LineItem { get; set; }

    /// <summary>
    /// The unique identifier of the refund line item.
    /// </summary>
    [JsonPropertyName("line_item_id")]
    public long? LineItemId { get; set; }

    /// <summary>
    /// The quantity of the associated line item that was returned.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; set; }

    /// <summary>
    /// Tax amount refunded
    /// </summary>
    [JsonPropertyName("total_tax")]
    public decimal? TotalTax { get; set; }

    /// <summary>
    /// Item subtotal
    /// </summary>
    [JsonPropertyName("subtotal")]
    public decimal? SubTotal { get; set; }

    /// <summary>
    /// The subtotal of the refund line item in shop and presentment currencies.
    /// </summary>
    [JsonPropertyName("subtotal_set")]
    public PriceSet? SubTotalTaxSet { get; set; }

    /// <summary>
    /// The total tax of the line item in shop and presentment currencies.
    /// </summary>
    [JsonPropertyName("total_tax_set")]
    public PriceSet? TotalTaxSet { get; set; }

    /// <summary>
    /// How this refund line item affects inventory levels.
    /// </summary>
    [JsonPropertyName("restock_type")]
    public string? RestockType { get; set; }

    /// <summary>
    /// The unique identifier of the location where the items will be restocked.
    /// Required when restock_type has the value return or cancel.
    /// </summary>
    [JsonPropertyName("location_id")]
    public long? LocationId { get; set; }
}