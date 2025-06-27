namespace VideoTag.Controllers.V2.Card;

public sealed record UpdateVideoRequest
{
    public required string ApiVideoId { get; init; }
    public required string Identifier { get; init; }
    public required string Title { get; init; }
}
