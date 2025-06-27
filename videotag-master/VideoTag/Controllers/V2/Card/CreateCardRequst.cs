namespace VideoTag.Controllers.V2.Card;

public sealed record CreateCardRequest
{
    public string? Url { get; init; }
}