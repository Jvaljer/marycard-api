namespace Common.Config;

public sealed record ShopifyConfig
{
    public required string ShopDomain { get; set; }
    public required string AccessToken { get; set; }
    public required string WebhookSecret { get; set; }
}