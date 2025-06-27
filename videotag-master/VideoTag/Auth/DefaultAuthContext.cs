using System.Security.Claims;
using AryDotNet.Authorization;

namespace VideoTag.Auth;

internal sealed class DefaultAuthContext(IHttpContextAccessor httpContextAccessor) : IAuthContext
{
    public bool IsUserConnected() => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid ConnectedUserId() => new(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                         throw new ArgumentNullException());

    public string[] ConnectedUserRoles() =>
        httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray() ?? [];
}
