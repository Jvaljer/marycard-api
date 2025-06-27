using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Commands;

public sealed record DeleteVariantsCommand : ICommand
{
    public required ulong ProductId { get; init; }
}

internal sealed class DeleteVariantsHandler(InventoryDbContext dbContext)
    : ICommandHandler<DeleteVariantsCommand>
{
    public async Task<Result<MessagingError>> Handle(DeleteVariantsCommand request, CancellationToken cancellationToken)
    {
        await dbContext.Set<Product>()
            .Where(p => p.ShopifyProductId.ProductId == request.ProductId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Deleted, true),
                cancellationToken: cancellationToken);
        return Result<MessagingError>.Ok();
    }
}