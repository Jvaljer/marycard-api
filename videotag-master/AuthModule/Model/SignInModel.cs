namespace AuthModule.Model;

public sealed record SignInModel
{
    public required bool SignedIn { get; init; }
    public required bool LockedOut { get; init; }
    public required bool EmailVerificationRequired { get; init; }
    public ICollection<string> Roles { get; init; } = new List<string>();

    public static SignInModel ValidCredentials(ICollection<string> roles) => new()
    {
        LockedOut = false,
        SignedIn = true,
        EmailVerificationRequired = false,
        Roles = roles
    };

    public static SignInModel InvalidCredentials() => new()
    {
        LockedOut = false,
        SignedIn = false,
        EmailVerificationRequired = false,
    };

    public static SignInModel AccountLocked() => new()
    {
        LockedOut = true,
        SignedIn = false,
        EmailVerificationRequired = false,
    };

    public static SignInModel EmailNotVerified() => new()
    {
        LockedOut = false,
        SignedIn = false,
        EmailVerificationRequired = true,
    };
}