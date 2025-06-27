using VideoModule.Domain.Entities;
using VideoModule.Models;

namespace VideoModule.Application;

internal static class Mapper
{
    public static CardModel MapCardToCardModel(Card card) => new()
    {
        Id = card.Identifier,
        Locked = card.Locked,
        Video = card.Video is null ? null : MapVideoFromVideoModel(card.Video),
        PreviewVideo = card.PreviewVideo is null ? null : MapVideoFromVideoModel(card.PreviewVideo),
        Group = card.Group is null ? null : MapGroup(card.Group),
        Url = card.Url,
        CreatedAt = card.CreatedAt,
        UpdatedAt = card.UpdatedAt
    };

    public static GroupModel MapGroup(Group group) => new()
    {
        Id = group.Id,
        Name = group.Name,
        PreviewVideo = group.PreviewVideo is null ? null : MapVideoFromVideoModel(group.PreviewVideo),
        CreatedAt = group.CreatedAt,
        UpdatedAt = group.UpdatedAt
    };

    public static VideoModel MapVideoFromVideoModel(Video video) => new()
    {
        Id = video.Id,
        Title = video.Title,
        Playable = video.Playable,
        ApiVideoId = video.ApiVideoId,
        VideoUrl = video.VideoUrl,
        CreatedAt = video.CreatedAt,
        UpdatedAt = video.UpdatedAt
    };

    public static VideoModel MapVideoFromVideoModel(PreviewVideo video) => new()
    {
        Id = video.Id,
        Title = video.Title,
        Playable = video.Playable,
        ApiVideoId = video.ApiVideoId,
        VideoUrl = video.VideoUrl,
        CreatedAt = video.CreatedAt,
        UpdatedAt = video.UpdatedAt
    };
}