using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetOrderByItemId : IQuery<OrderModel>
{
    public required Guid ItemId { get; init; }
}

internal sealed class GetOrderByItemIdHandle(OrderDbContext dbContext) : IQueryHandler<GetOrderByItemId, OrderModel>
{
    public async Task<Result<OrderModel, MessagingError>> Handle(GetOrderByItemId request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<OrderItem>()
            .AsNoTracking()
            .Include(item => item.Product)
            .ThenInclude(product => product!.Order)
            .Where(item => item.PhysicalCardId == request.ItemId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "No order found.");
        }

        return Mapper.OrderToModel(order.Product!.Order!);
    }
}