using Microsoft.AspNetCore.Http;

namespace AuthModule.Config;

internal sealed class AuthConfig
{
    public required bool RequireConfirmedAccount { get; set; }
    public required int MaxFailedAccessAttempts { get; set; }
    public required int DefaultLockoutTimeSpan { get; set; }
    public required int CookieExpireTimeSpan { get; set; }
    public required SameSiteMode SameSite { get; set; }
}