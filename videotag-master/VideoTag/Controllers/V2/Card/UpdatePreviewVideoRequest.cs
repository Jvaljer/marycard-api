namespace VideoTag.Controllers.V2.Card;

public sealed record UpdatePreviewVideoRequest
{
    public required Guid? VideoId { get; init; }
}