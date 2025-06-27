using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetOrderByShopifyId : IQuery<OrderModel>
{
    public required ulong ShopifyOrderId { get; init; }
}

internal sealed class GetOrderByShopifyIdHandler(OrderDbContext dbContext) : IQueryHandler<GetOrderByShopifyId, OrderModel>
{
    public async Task<Result<OrderModel, MessagingError>> Handle(GetOrderByShopifyId request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Products)
            .ThenInclude(p => p.Items)
            .FirstOrDefaultAsync(o => o.ShopifyOrderId == request.ShopifyOrderId, cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order not found.");
        }

        return Mapper.OrderToModel(order);
    }
}