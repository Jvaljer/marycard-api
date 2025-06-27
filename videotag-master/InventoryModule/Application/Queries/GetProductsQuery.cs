using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetProductsQuery : IQuery<ICollection<ProductModel>>
{
    public required PageQuery Page { get; init; }
}

internal sealed class GetProductsHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetProductsQuery, ICollection<ProductModel>>
{
    public async Task<Result<ICollection<ProductModel>, MessagingError>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Set<Product>().AsNoTracking()
            .Where(x => !x.Deleted)
            .PagedOrderedDescending(request.Page, product => product.CreatedAt)
            .ToListAsync(cancellationToken);

        return products.Select(Mapper.ProductToProductModel).ToList();
    }
}