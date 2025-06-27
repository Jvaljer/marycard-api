namespace VideoTag.Controllers.Sales;

public sealed record DeleteRequest
{
    public required ulong Id { get; init; }
}
