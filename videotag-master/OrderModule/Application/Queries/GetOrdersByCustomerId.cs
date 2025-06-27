using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetOrdersByCustomerId : IQuery<ICollection<OrderModel>>
{
    public required ulong CustomerId { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetOrdersByCustomerIdHandler(OrderDbContext dbContext) : IQueryHandler<GetOrdersByCustomerId, ICollection<OrderModel>>
{
    public async Task<Result<ICollection<OrderModel>, MessagingError>> Handle(GetOrdersByCustomerId request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Set<Order>()
            .AsNoTracking()
            .Include(order => order.Products)
            .ThenInclude(orderProduct => orderProduct.Items)
            .Where(order => order.ShopifyCustomerId == request.CustomerId)
            .PagedOrderedDescending(request.Page)
            .ToListAsync(cancellationToken);

        return orders.Select(Mapper.OrderToModel).ToList();
    }
}