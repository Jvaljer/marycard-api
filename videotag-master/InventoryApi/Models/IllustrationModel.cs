namespace InventoryApi.Models;

public sealed record IllustrationModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? ImageUrl { get; init; }
    public required float Width { get; init; }
    public required float Height { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}