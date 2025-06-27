namespace VideoTag.Controllers.V2.Card;

public sealed record UpdateCardGroupRequest
{
    public Guid? GroupId { get; init; }
}