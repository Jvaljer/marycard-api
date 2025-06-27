using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetRecentOrders : IQuery<ICollection<OrderModel>>
{
    public required OrderState? State { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetRecentOrdersHandler(OrderDbContext dbContext)
    : IQueryHandler<GetRecentOrders, ICollection<OrderModel>>
{
    public async Task<Result<ICollection<OrderModel>, MessagingError>> Handle(GetRecentOrders request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Order>()
                .AsNoTracking()
                .Include(order => order.Products)
                .ThenInclude(orderProduct => orderProduct.Items)
                .AsQueryable();

        if (request.State is not null)
        {
            query = query.Where(order => order.State == request.State);
        }

        var orders = await query
            .PagedOrderedDescending(request.Page)
            .ToListAsync(cancellationToken);

        return orders.Select(Mapper.OrderToModel).ToList();
    }
}