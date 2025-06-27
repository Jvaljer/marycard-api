namespace VideoTag.Controllers.V2.Auth;

public sealed record SignInRequest
{
    public required string Identifier { get; init; }
    public required string Password { get; init; }
}