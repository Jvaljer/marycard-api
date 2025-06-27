namespace VideoTag.Controllers.V2.Group;

public sealed record CreateGroupRequest
{
    public required string Name { get; init; }
    public Guid? PreviewVideoId { get; init; }
}