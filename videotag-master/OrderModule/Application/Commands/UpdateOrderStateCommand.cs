using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record UpdateOrderStateCommand : ICommand
{
    public required Guid OrderId { get; init; }
    public required OrderState State { get; init; }
}

internal sealed class UpdateOrderStateHandler(OrderDbContext dbContext) : ICommandHandler<UpdateOrderStateCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateOrderStateCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<Order>()
            .FirstOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order not found.");
        }

        order.State = request.State;
        dbContext.Set<Order>().Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}