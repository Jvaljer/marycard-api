namespace VideoTag.Controllers.V2.Card;

public sealed record UpdateCardUrlRequest
{
    public string? Url { get; init; }
}