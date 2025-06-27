using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common.Config;
using Common.Shopify;
using InventoryApi.Models;
using InventoryModule.Application.Commands;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using ShopifySharp;
using ShopifySharp.Factories;
using ShopifySharp.Filters;

namespace InventoryModule.BackgroundServices;

/// <summary>
/// Synchronizes products from Shopify to the database.
/// </summary>
internal sealed class ProductSyncService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private static DateTimeOffset _lastSyncTime = DateTimeOffset.MinValue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var initialScope = serviceScopeFactory.CreateScope();
        var productServiceFactory = initialScope.ServiceProvider.GetRequiredService<IProductServiceFactory>();
        var options = initialScope.ServiceProvider.GetRequiredService<IOptions<ShopifyConfig>>();
        var productService = productServiceFactory.Create(options.Value.ShopDomain, options.Value.AccessToken);
        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

        Log.Information("{Service}: Starting product sync service", nameof(ProductSyncService));
        var firstJobRepository = initialScope.ServiceProvider.GetRequiredService<IRepository<SyncJob>>();

        var lastJob = await firstJobRepository.Query().AsNoTracking().OrderByDescending(job => job.CreatedAt).FirstOrDefaultAsync(j => j.Status == SyncJobStatus.Success, stoppingToken);
        if (lastJob is not null)
        {
            Log.Information("{Service}: Last successful sync job was at {CreatedAt}", nameof(ProductSyncService), lastJob.CreatedAt);
            _lastSyncTime = lastJob.CreatedAt;
        }
        else
        {
            Log.Information("{Service}: Syncing all products", nameof(ProductSyncService));
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            using var loopScope = serviceScopeFactory.CreateScope();
            var dbContext = loopScope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            var jobRepository = loopScope.ServiceProvider.GetRequiredService<IRepository<SyncJob>>();
            var sender = loopScope.ServiceProvider.GetRequiredService<ISender>();

            Log.Information("{Service}: Syncing products from Shopify", nameof(ProductSyncService));
            var status = await SyncProducts(sender, productService, stoppingToken);
            Log.Information("{Service}: Sync job finished with status {@Status}", nameof(ProductSyncService), status);

            _lastSyncTime = DateTimeOffset.UtcNow;
            await jobRepository.AddAsync(new SyncJob { Status = status }, stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }

    private async Task<Result<List<ProductModel>, MessagingError>> CreateOrUpdateProducts(ISender sender, IEnumerable<ShopifySharp.Product> products, CancellationToken cancellationToken)
    {
        var contentModels = products.SelectMany(p => p.Variants.Select(variant => new ProductContentModel
        {
            ShopifyProductId = new ShopifyProductId
            {
                ProductId = (ulong)p.Id!,
                VariantId = (ulong)variant.Id!
            },
            SKU = variant.SKU,
            Name = p.Title,
            VariantName = variant.Title,
            Description = p.BodyHtml ?? ""
        })).ToList();
        return await sender.Send(new CreateOrUpdateProductsCommand
        {
            Products = contentModels
        }, cancellationToken);
    }

    private async Task<SyncJobStatus> SyncProducts(ISender sender, IProductService productService, CancellationToken cancellationToken)
    {
        ListFilter<ShopifySharp.Product>? filter = new ProductListFilter
        {
            UpdatedAtMin = _lastSyncTime,
            Limit = 10
        };

        var page = 0;
        while (filter is not null)
        {
            var products = await productService.ListAsync(filter, cancellationToken: cancellationToken);
            if (products is null)
            {
                Log.Error("{Service}: Failed to fetch products from Shopify at page {Page}", nameof(ProductSyncService), page);
                return SyncJobStatus.Failed;
            }

            if (!products.Items.Any())
            {
                return SyncJobStatus.Success;
            }

            var result = await CreateOrUpdateProducts(sender, products.Items, cancellationToken);
            if (result.IsError)
            {
                Log.Error("{Service}: Failed to create or update products: {@Result}", nameof(ProductSyncService), result);
                return SyncJobStatus.Failed;
            }

            filter = products.GetNextPageFilter();
            Log.Information("{Service}: Successfully synced page {Page} of products", nameof(ProductSyncService), page);
            page++;
        }

        return SyncJobStatus.Success;
    }
}