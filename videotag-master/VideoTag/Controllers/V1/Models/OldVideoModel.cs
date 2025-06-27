using VideoModule.Models;

namespace VideoTag.Controllers.V1.Models;

public sealed record OldVideoModel
{
    public required Guid Id { get; init; }
    public required string Identifier { get; init; }
    public required string Title { get; init; }
    public required string VideoUrl { get; init; }
    public required bool Playable { get; init; }
    public required bool Locked { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }

    public static OldVideoModel? FromCard(CardModel cardModel) => cardModel.Video is null
        ? null
        : new OldVideoModel
        {
            Id = Guid.Empty,
            Identifier = cardModel.Id,
            Locked = cardModel.Locked,
            Playable = cardModel.Video.Playable,
            Title = cardModel.Video.Title,
            VideoUrl = cardModel.Video.VideoUrl,
            CreatedAt = cardModel.CreatedAt.DateTime,
            UpdatedAt = cardModel.UpdatedAt.DateTime
        };
}