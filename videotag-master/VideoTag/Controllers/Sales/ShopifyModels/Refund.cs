using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class Refund
{
    /// <summary>
    /// The unique identifier of the order.
    /// </summary>
    [JsonPropertyName("order_id")]
    public long? OrderId { get; set; }

    /// <summary>
    /// The date and time when the refund was created. 
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    ///<summary>
    /// Whether to send a refund notification to the customer
    /// </summary>
    [JsonPropertyName("notify")]
    public bool? Notify { get; set; }

    /// <summary>
    /// Specify how much shipping to refund.
    /// </summary>
    [JsonPropertyName("shipping")]
    public Shipping? Shipping { get; set; }

    /// <summary>
    /// The three-letter code (ISO 4217 format) for the currency used for the refund. Note: Required whenever the shipping amount property is provided.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// The list of <see cref="RefundOrderAdjustment"/> objects
    /// </summary>
    [JsonPropertyName("order_adjustments")]
    public IEnumerable<RefundOrderAdjustment>? OrderAdjustments { get; set; }

    /// <summary>
    /// The date and time when the refund was imported.
    /// </summary>
    /// <remarks>
    /// This value can be set to dates in the past when importing from other systems. If no value is provided, it will be auto-generated.
    /// </remarks>
    [JsonPropertyName("processed_at")]
    public DateTimeOffset? ProcessedAt { get; set; }

    /// <summary>
    /// The optional note attached to a refund.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>
    /// An optional comment that explains a discrepancy between calculated and actual refund amounts. 
    /// Used to populate the reason property of the resulting order adjustment object attached to the refund.
    /// </summary>
    /// <value>restock, damage, customer, and other.</value>
    [JsonPropertyName("discrepancy_reason")]
    public string? DiscrepancyReason { get; set; }

    /// <summary>
    /// The list of <see cref="RefundLineItem"/> objects
    /// </summary>
    [JsonPropertyName("refund_line_items")]
    public IEnumerable<RefundLineItem>? RefundLineItems { get; set; }

    /// <summary>
    /// The list of <see cref="Transaction"/> objects
    /// </summary>
    [JsonPropertyName("transactions")]
    public IEnumerable<Transaction>? Transactions { get; set; }

    /// <summary>
    /// The unique identifier of the user who performed the refund.
    /// </summary>
    [JsonPropertyName("user_id")]
    public long? UserId { get; set; }

    /// <summary>
    /// A list of duties that have been returned as part of the refund.
    /// </summary>
    [JsonPropertyName("duties")]
    public IEnumerable<RefundDuty>? Duties { get; set; }

    /// <summary>
    /// A list of refunded duties
    /// </summary>
    [JsonPropertyName("refund_duties")]
    public IEnumerable<RefundDutyType>? RefundDuties { get; set; }

    /// <summary>
    /// Unique identifiers for the return.
    /// </summary>
    [JsonPropertyName("return")]
    public Return? Return { get; set; }
}

public class Shipping
{
    /// <summary>
    /// Whether to refund all remaining shipping.
    /// </summary>
    [JsonPropertyName("full_refund")]
    public bool? FullRefund { get; set; }

    /// <summary>
    /// Set a specific amount to refund for shipping. Takes precedence over full_refund.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// The maximum amount that can be refunded
    /// </summary>
    [JsonPropertyName("maximum_refundable")]
    public decimal? MaximumRefundable { get; set; }
}