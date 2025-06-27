using Common.Shopify;

namespace Common;

public static class Constants
{
    public const uint DefaultPageSize = 20;
    public const int FileUploadMaxSize = 104857600; // 100MB
    public const int ShopifyFakeOrderId = 0x0;

    public static readonly ShopifyProductId ShopifyFakeProductId = new()
    {
        ProductId = 0x0,
        VariantId = 0x0
    };

    public const int ShopifyFakeCustomerId = 0x0;
}