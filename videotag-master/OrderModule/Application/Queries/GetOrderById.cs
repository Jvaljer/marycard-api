using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetOrderById : IQuery<OrderModel>
{
    public required Guid OrderId { get; init; }
}

internal sealed class GetOrderByIdHandler(OrderDbContext dbContext) : IQueryHandler<GetOrderById, OrderModel>
{
    public async Task<Result<OrderModel, MessagingError>> Handle(GetOrderById request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Products)
            .ThenInclude(p => p.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order not found.");
        }

        return Mapper.OrderToModel(order);
    }
}