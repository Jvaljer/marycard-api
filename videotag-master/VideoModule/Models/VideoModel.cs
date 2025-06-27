namespace VideoModule.Models;

public sealed record VideoModel
{
    public required Guid Id { get; init; }
    public required string ApiVideoId { get; init; }
    public required string Title { get; init; }
    public required string VideoUrl { get; init; }
    public required bool Playable { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}