namespace VideoTag.Controllers.V2.Inventory;

public sealed record UpdatePhysicalCardRedirectionRequest
{
    public required string Url { get; init; }
}