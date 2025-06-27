using OrderAPI.Models;
using OrderModule.Domain;

namespace OrderModule.Application;

internal static class Mapper
{
    public static OrderModel OrderToModel(Order order) => new()
    {
        Id = order.Id,
        ShopifyOrderId = order.ShopifyOrderId,
        ShopifyCustomerId = order.ShopifyCustomerId,
        CustomerEmail = order.CustomerEmail,
        ContactEmail = order.ContactEmail,
        CustomerPhone = order.CustomerPhone,
        CustomerFirstName = order.CustomerFirstName,
        CustomerLastName = order.CustomerLastName,
        Note = order.Note,
        State = order.State,
        ShopifyCreatedAt = order.ShopifyCreatedAt,
        ShopifyUpdatedAt = order.ShopifyUpdatedAt,
        Products = order.Products.Select(OrderProductToModel).ToList(),
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt
    };

    private static OrderProductModel OrderProductToModel(OrderProduct product) => new()
    {
        Id = product.Id,
        ShopifyProductId = product.ShopifyProductId,
        Quantity = product.Quantity,
        Items = product.Items.Select(OrderItemToModel).ToList(),
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };

    private static OrderItemModel OrderItemToModel(OrderItem item) => new()
    {
        PhysicalCardId = item.PhysicalCardId,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };
}