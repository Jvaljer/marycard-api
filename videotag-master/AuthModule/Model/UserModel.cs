namespace AuthModule.Model;

public sealed record UserModel
{
    public required Guid Id { get; init; }
    public string? Email { get; init; }
    public required string Identifier { get; init; }
    public required ICollection<string> Roles { get; init; }
}