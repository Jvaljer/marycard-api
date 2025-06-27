namespace VideoTag.Controllers.V2.Card;

public sealed record CreateVideoRequest
{
    public required string ApiVideoId { get; init; }
    public required string Title { get; init; }
}