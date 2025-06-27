using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Commands;

public sealed record CreateOrUpdateProductsCommand : ICommand<List<ProductModel>>
{
    public required List<ProductContentModel> Products { get; init; }
}

internal sealed class CreateOrUpdateProductHandler(InventoryDbContext dbContext)
    : ICommandHandler<CreateOrUpdateProductsCommand, List<ProductModel>>
{
    public async Task<Result<List<ProductModel>, MessagingError>> Handle(CreateOrUpdateProductsCommand request,
        CancellationToken cancellationToken)
    {
        var productModels = new List<ProductModel>();
        foreach (var productModel in request.Products)
        {
            var existingProduct = await dbContext.Set<Product>().FirstOrDefaultAsync(
                p => p.ShopifyProductId.ProductId == productModel.ShopifyProductId.ProductId &&
                     p.ShopifyProductId.VariantId == productModel.ShopifyProductId.VariantId,
                cancellationToken);
            if (existingProduct is not null)
            {
                existingProduct.Name = productModel.Name ?? existingProduct.Name;
                existingProduct.Description = productModel.Description ?? existingProduct.Description;
                existingProduct.VariantName = productModel.VariantName ?? existingProduct.VariantName;
                existingProduct.SKU = productModel.SKU;
                existingProduct.Deleted = false;
                dbContext.Set<Product>().Update(existingProduct);
                productModels.Add(Mapper.ProductToProductModel(existingProduct));
            }
            else
            {
                var newProduct = new Product
                {
                    Name = productModel.Name ?? "Name not set",
                    Description = productModel.Description ?? "",
                    ShopifyProductId = productModel.ShopifyProductId,
                    VariantName = productModel.VariantName ?? "Variant not set",
                    SKU = productModel.SKU,
                    Deleted = false,
                };
                await dbContext.Set<Product>().AddAsync(newProduct, cancellationToken);
                productModels.Add(Mapper.ProductToProductModel(newProduct));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return productModels;
    }
}