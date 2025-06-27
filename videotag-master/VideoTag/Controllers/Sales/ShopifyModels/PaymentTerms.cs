using System.Text.Json.Serialization;

namespace VideoTag.Controllers.Sales.ShopifyModels;

public sealed class PaymentTerms
{
    /// <summary>
    /// The amount that is owed according to the payment terms.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? amount { get; set; }

    /// <summary>
    /// The presentment currency for the payment
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// The number of days between the invoice date and due date that is defined in the selected payment terms template.
    /// </summary>
    [JsonPropertyName("due_in_days")]
    public int? DueInDays { get; set; }

    /// <summary>
    /// The name of the selected payment terms template for the order.
    /// </summary>
    [JsonPropertyName("payment_terms_name")]
    public string? PaymentTermsName { get; set; }

    /// <summary>
    /// The type of selected payment terms template for the order.
    /// </summary>
    [JsonPropertyName("payment_terms_type")]
    public string? PaymentTermsType { get; set; }

    /// <summary>
    /// An array of schedules associated to the payment terms.
    /// </summary>
    [JsonPropertyName("payment_schedules")]
    public PaymentSchedule[]? PaymentSchedules { get; set; }
}