namespace VideoModule.Models;

public sealed record GroupModel
{
    public required Guid Id { get; init; }
    public required VideoModel? PreviewVideo { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
}