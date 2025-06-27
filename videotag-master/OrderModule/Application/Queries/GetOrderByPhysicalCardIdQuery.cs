using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record GetOrderByPhysicalCardIdQuery : IQuery<OrderModel>
{
    public required Guid PhysicalCardId { get; init; }
}

internal sealed class GetOrderByCardIdHandler(OrderDbContext dbContext) : IQueryHandler<GetOrderByPhysicalCardIdQuery, OrderModel>
{
    public async Task<Result<OrderModel, MessagingError>> Handle(GetOrderByPhysicalCardIdQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<OrderItem>()
            .AsNoTracking()
            .Include(item => item.Product)
            .ThenInclude(product => product!.Order)
            .FirstOrDefaultAsync(item => item.PhysicalCardId == request.PhysicalCardId, cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order not found");
        }

        return Mapper.OrderToModel(order.Product!.Order!);
    }
}