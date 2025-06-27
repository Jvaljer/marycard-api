using TagClient.Models.Tag;

namespace VideoTag.Controllers.V2.Tag;


public sealed record TagRedirectedModel
{
    public required TagModel Tag { get; init; }
    public required string RedirectUrl { get; init; }
}