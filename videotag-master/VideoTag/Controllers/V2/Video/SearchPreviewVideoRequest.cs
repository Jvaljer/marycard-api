namespace VideoTag.Controllers.V2.Video;

public record SearchPreviewVideoRequest
{
    public required string Query { get; init; }
}