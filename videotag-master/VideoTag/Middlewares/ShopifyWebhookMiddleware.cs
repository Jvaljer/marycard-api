using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ShopifySharp.Utilities;

namespace VideoTag.Middlewares;

internal sealed class ShopifyAuthenticationOptions : AuthenticationSchemeOptions { }

internal sealed class ShopifyAuthenticationHandler(
    IOptionsMonitor<ShopifyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration, IShopifyRequestValidationUtility shopifyValidator, IWebHostEnvironment environment
    )
    : AuthenticationHandler<ShopifyAuthenticationOptions>(options, logger, encoder)
{
    public const string SchemeName = "ShopifyWebhook";

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        await Response.WriteAsJsonAsync(new
        {
            Success = false,
            Error = "Unauthorized"
        });
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!environment.IsProduction())
        {
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(Scheme.Name)), Scheme.Name));
        }

        var webhookSecret = configuration["Shopify:WebhookSecret"];
        if (string.IsNullOrEmpty(webhookSecret))
        {
            return AuthenticateResult.Fail("Webhook secret is not configured.");
        }

        Request.EnableBuffering();

        var isAuthentic = await shopifyValidator.IsAuthenticWebhookAsync(Request.Headers, Request.Body, webhookSecret);

        if (Request.Body.CanSeek)
        {
            Request.Body.Position = 0;
        }

        if (!isAuthentic)
        {
            return AuthenticateResult.Fail("Request does not pass Shopify's validation scheme.");
        }

        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(Scheme.Name));
        return AuthenticateResult.Success(new AuthenticationTicket(claimPrincipal, Scheme.Name));
    }
}