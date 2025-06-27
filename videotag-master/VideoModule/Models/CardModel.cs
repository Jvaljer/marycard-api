namespace VideoModule.Models;

public record CardModel
{
    public required string Id { get; init; }
    public required VideoModel? Video { get; init; }
    public required VideoModel? PreviewVideo { get; init; }
    public required GroupModel? Group { get; init; }
    public required string? Url { get; init; }
    public required bool Locked { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
}