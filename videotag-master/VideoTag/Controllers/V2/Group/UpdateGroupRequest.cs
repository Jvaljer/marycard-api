namespace VideoTag.Controllers.V2.Group;

public sealed record UpdateGroupRequest
{
    public required string Name { get; init; }
    public required Guid? PreviewVideoId { get; init; }
}