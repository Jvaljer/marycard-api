namespace VideoTag.Controllers.V2.Order;

public sealed record CreateOrderRequest
{
    public string? ContactEmail { get; init; }
    public string? CustomerEmail { get; init; }
    public string? Note { get; init; }
    public string? CustomerPhone { get; init; }
    public string? CustomerFirstName { get; init; }
    public string? CustomerLastName { get; init; }
}