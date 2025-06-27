using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common;
using InventoryApi.Models;
using InventoryModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Events;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record CreateOrderItemCommand : ICommand
{
    public required Trusted<PhysicalCardModel> PhysicalCard { get; init; }
    public required Guid OrderProductId { get; init; }
}

internal sealed class CreateOrderItemHandler(OrderDbContext dbContext, IPublisher publisher)
    : ICommandHandler<CreateOrderItemCommand>
{
    public async Task<Result<MessagingError>> Handle(CreateOrderItemCommand request,
        CancellationToken cancellationToken)
    {
        var physicalCardAlreadyInUse = await dbContext.Set<OrderItem>()
            .AnyAsync(orderItem => orderItem.PhysicalCardId == request.PhysicalCard.Value.Id, cancellationToken);

        if (physicalCardAlreadyInUse)
        {
            return new MessagingError(HttpStatusCode.Conflict,
                $"Physical card with ID {request.PhysicalCard.Value.Id} is already associated with an order item.");
        }

        var product = await dbContext.Set<OrderProduct>()
            .FirstOrDefaultAsync(product => product.Id == request.OrderProductId, cancellationToken);

        if (product is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order product not found.");
        }

        await dbContext.Set<OrderItem>().AddAsync(new OrderItem
        {
            PhysicalCardId = request.PhysicalCard.Value.Id,
            OrderProductId = request.OrderProductId,
        }, cancellationToken);

        product.UpdatedAt = DateTimeOffset.UtcNow.DateTime;

        dbContext.Update(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        await publisher.Publish(new PhysicalCardAssociatedWithOrderProduct
        {
            OrderProductId = product.Id,
            PhysicalCardId = request.PhysicalCard.Value.Id,
            ShopifyProductId = product.ShopifyProductId,
            VideoCardId = request.PhysicalCard.Value.VideoCardId,
            OrderId = product.OrderId,
        }, cancellationToken);

        return Result<MessagingError>.Ok();
    }
}