using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Queries;

public sealed record SearchOrders : IQuery<ICollection<OrderModel>>
{
    public required string Text { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class SearchOrdersHandler(OrderDbContext dbContext)
    : IQueryHandler<SearchOrders, ICollection<OrderModel>>
{
    public async Task<Result<ICollection<OrderModel>, MessagingError>> Handle(SearchOrders request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Order>().AsNoTracking();

        // We support search through Shopify IDs
        // so we try to parse the input as a Shopify ID.
        if (!request.Text.IsNullOrEmpty() && ulong.TryParse(request.Text, out var shopifyId))
        {
            query = query.Where(order => order.ShopifyCustomerId == shopifyId || order.ShopifyOrderId == shopifyId);
        }
        else if (!request.Text.IsNullOrEmpty())
        {
            var normalizedText = request.Text.ToLowerInvariant();
            query = query.Where(order =>
                (order.CustomerFirstNameNormalized != null
                    && order.CustomerFirstNameNormalized.Contains(normalizedText)) ||
                (order.CustomerLastNameNormalized != null
                    && order.CustomerLastNameNormalized.Contains(normalizedText)) ||
                (order.CustomerEmailNormalized != null
                    && order.CustomerEmailNormalized.Contains(normalizedText)) ||
                (order.CustomerPhone != null
                    && order.CustomerPhone.Contains(normalizedText)) ||
                (order.ContactEmailNormalized != null
                    && order.ContactEmailNormalized.Contains(normalizedText)) ||
                (order.NoteNormalized != null
                    && order.NoteNormalized.Contains(normalizedText)));
        }

        var orders = await query
                .Include(order => order.Products)
                .ThenInclude(orderProduct => orderProduct.Items)
                .PagedOrderedDescending(request.Page)
                .ToListAsync(cancellationToken);

        return orders.Select(Mapper.OrderToModel).ToList();
    }
}