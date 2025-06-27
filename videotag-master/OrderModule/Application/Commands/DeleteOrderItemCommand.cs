using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Events;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record DeleteOrderItemCommand : ICommand
{
    public required Guid PhysicalCardId { get; init; }
    public required Guid OrderId { get; init; }
}

internal sealed class DeleteOrderItemHandler(OrderDbContext dbContext, IPublisher publisher)
    : ICommandHandler<DeleteOrderItemCommand>
{
    public async Task<Result<MessagingError>> Handle(DeleteOrderItemCommand request,
        CancellationToken cancellationToken)
    {
        var item = await dbContext.Set<OrderItem>()
            .Include(item => item.Product)
            .Where(item => item.PhysicalCardId == request.PhysicalCardId && item.Product!.OrderId == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (item is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order item not found.");
        }

        dbContext.Set<OrderItem>().Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        await publisher.Publish(new PhysicalCardUnassociatedWithOrderProduct
        {
            OrderId = item.Product!.OrderId,
            ProductId = item.OrderProductId,
            PhysicalCardId = item.PhysicalCardId,
        }, cancellationToken);
        return Result<MessagingError>.Ok();
    }
}