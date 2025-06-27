using AryDotNet.Messaging;
using AryDotNet.Result;
using Common;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record CreateFakeOrderCommand : ICommand<Guid>
{
    public required string? ContactEmail { get; init; }
    public required string? CustomerEmail { get; init; }
    public required string? Note { get; init; }
    public required string? CustomerPhone { get; init; }
    public required string? CustomerFirstName { get; init; }
    public required string? CustomerLastName { get; init; }
}

internal sealed class CreateFakeOrderCommandHandler(OrderDbContext dbContext) : ICommandHandler<CreateFakeOrderCommand, Guid>
{
    public async Task<Result<Guid, MessagingError>> Handle(CreateFakeOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ShopifyOrderId = Constants.ShopifyFakeOrderId,
            ShopifyCustomerId = Constants.ShopifyFakeCustomerId,
            State = OrderState.ProductsNotScanned,
            CustomerEmail = request.CustomerEmail,
            CustomerEmailNormalized = request.CustomerEmail?.ToUpperInvariant(),
            ContactEmail = request.ContactEmail,
            ContactEmailNormalized = request.ContactEmail?.ToUpperInvariant(),
            CustomerPhone = request.CustomerPhone,
            CustomerFirstName = request.CustomerFirstName,
            CustomerFirstNameNormalized = request.CustomerFirstName?.ToUpperInvariant(),
            CustomerLastName = request.CustomerLastName,
            CustomerLastNameNormalized = request.CustomerLastName?.ToUpperInvariant(),
            Note = request.Note,
            NoteNormalized = request.Note?.ToUpperInvariant(),
            ShopifyCreatedAt = DateTime.UtcNow,
            ShopifyUpdatedAt = DateTime.UtcNow
        };

        await dbContext.Set<Order>().AddAsync(order, cancellationToken);

        var orderProduct = new OrderProduct
        {
            OrderId = order.Id,
            ShopifyProductId = Constants.ShopifyFakeProductId,
            Quantity = 10000000
        };

        await dbContext.Set<OrderProduct>().AddAsync(orderProduct, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<Guid, MessagingError>.Ok(order.Id);
    }
}