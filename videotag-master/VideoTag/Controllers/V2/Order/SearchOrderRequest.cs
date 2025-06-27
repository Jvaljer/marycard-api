namespace VideoTag.Controllers.V2.Order;

public record SearchOrderRequest
{
    public required string Text { get; init; }
}