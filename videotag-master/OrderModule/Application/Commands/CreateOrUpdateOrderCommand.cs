using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Shopify;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record ShopifyOrderProduct
{
    public required ShopifyProductId ShopifyProductId { get; init; }
    public required int Quantity { get; init; }
}

public sealed record CreateOrUpdateOrderCommand : ICommand<Guid>
{
    public required ulong ShopifyOrderId { get; init; }
    public required ulong ShopifyCustomerId { get; init; }
    public string? CustomerEmail { get; init; }
    public string? ContactEmail { get; init; }
    public string? CustomerPhone { get; init; }
    public string? CustomerFirstName { get; init; }
    public string? CustomerLastName { get; init; }
    public required DateTime ShopifyCreatedAt { get; init; }
    public required DateTime ShopifyUpdatedAt { get; init; }
    public required List<ShopifyOrderProduct> Products { get; init; }
}

internal sealed class CreateOrderHandler(
    OrderDbContext dbContext) : ICommandHandler<CreateOrUpdateOrderCommand, Guid>
{
    private async Task<Guid> CreateOrder(CreateOrUpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ShopifyCustomerId = request.ShopifyCustomerId,
            ShopifyOrderId = request.ShopifyOrderId,
            ContactEmail = request.ContactEmail,
            ContactEmailNormalized = request.ContactEmail?.ToUpperInvariant(),
            CustomerEmail = request.CustomerEmail,
            CustomerEmailNormalized = request.CustomerEmail?.ToUpperInvariant(),
            CustomerPhone = request.CustomerPhone,
            CustomerFirstName = request.CustomerFirstName,
            CustomerFirstNameNormalized = request.CustomerFirstName?.ToUpperInvariant(),
            CustomerLastName = request.CustomerLastName,
            CustomerLastNameNormalized = request.CustomerLastName?.ToUpperInvariant(),
            ShopifyCreatedAt = request.ShopifyCreatedAt,
            ShopifyUpdatedAt = request.ShopifyUpdatedAt,
            State = OrderState.ProductsNotScanned
        };

        var createdOrderEntity = await dbContext.Set<Order>().AddAsync(order, cancellationToken);

        var products = request.Products.Select(p => new OrderProduct
        {
            ShopifyProductId = p.ShopifyProductId,
            Quantity = p.Quantity,
            OrderId = createdOrderEntity.Entity.Id,
        }).ToList();

        await dbContext.Set<OrderProduct>().AddRangeAsync(products, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return createdOrderEntity.Entity.Id;
    }

    private async Task UpdateOrder(Order order, CreateOrUpdateOrderCommand request, CancellationToken cancellationToken)
    {
        if (order.ShopifyUpdatedAt >= request.ShopifyUpdatedAt)
        {
            // The database already has the latest version of the order
            return;
        }

        order.ShopifyCustomerId = request.ShopifyCustomerId;
        order.ContactEmail = request.ContactEmail;
        order.ContactEmailNormalized = request.ContactEmail?.ToUpperInvariant();
        order.CustomerEmail = request.CustomerEmail;
        order.CustomerEmailNormalized = request.CustomerEmail?.ToUpperInvariant();
        order.CustomerPhone = request.CustomerPhone;
        order.CustomerFirstName = request.CustomerFirstName;
        order.CustomerFirstNameNormalized = request.CustomerFirstName?.ToUpperInvariant();
        order.CustomerLastName = request.CustomerLastName;
        order.CustomerLastNameNormalized = request.CustomerLastName?.ToUpperInvariant();
        order.ShopifyUpdatedAt = request.ShopifyUpdatedAt;
        order.UpdatedAt = DateTime.Now;


        dbContext.Set<OrderProduct>().RemoveRange(order.Products);

        var products = request.Products.Select(p => new OrderProduct
        {
            ShopifyProductId = p.ShopifyProductId,
            Quantity = p.Quantity,
            OrderId = order.Id,
        }).ToList();

        await dbContext.Set<OrderProduct>().AddRangeAsync(products, cancellationToken);

        dbContext.Set<Order>().Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<Guid, MessagingError>> Handle(CreateOrUpdateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var existingOrder = await dbContext.Set<Order>()
            .Include(order => order.Products)
            .FirstOrDefaultAsync(order => order.ShopifyOrderId == request.ShopifyOrderId, cancellationToken);

        if (existingOrder is not null)
        {
            await UpdateOrder(existingOrder, request, cancellationToken);
            return existingOrder.Id;
        }

        return await CreateOrder(request, cancellationToken);
    }
}