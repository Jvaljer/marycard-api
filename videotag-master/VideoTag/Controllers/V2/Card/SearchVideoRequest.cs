namespace VideoTag.Controllers.V2.Card;

public record SearchVideoRequest
{
    public string? Identifier { get; init; }
}