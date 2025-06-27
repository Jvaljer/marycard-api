using OrderAPI.Models;

namespace VideoTag.Controllers.V2.Order;

public sealed record UpdateOrderStateRequest
{
    public required OrderState OrderState { get; init; }
}