namespace VideoTag.Controllers.V1.Models;

public sealed record OldCreateVideoRequest
{
    public required string ApiVideoId { get; init; }
    public required string Title { get; init; }
    public required string Identifier { get; init; }
}