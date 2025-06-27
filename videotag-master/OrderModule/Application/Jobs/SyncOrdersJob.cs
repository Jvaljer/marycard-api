using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common;
using Common.Config;
using Common.Shopify;
using InventoryModule.Application.Commands;
using InventoryModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderModule.Application.Commands;
using OrderModule.Domain;
using OrderModule.Infrastructure;
using Serilog;
using ShopifySharp.Factories;
using ShopifySharp.Filters;
using Order = ShopifySharp.Order;

namespace OrderModule.Application.Jobs;

internal sealed record SyncOrdersJob : IJob;

internal sealed class SyncOrdersHandler(
    OrderDbContext dbContext,
    IOrderServiceFactory orderServiceFactory,
    IOptions<ShopifyConfig> shopifyOptions,
    ISender sender) : IJobHandler<SyncOrdersJob>
{
    public async Task<Result<MessagingError>> Handle(SyncOrdersJob request, CancellationToken cancellationToken)
    {
        Log.Information("{Job}: Syncing orders from Shopify", nameof(SyncOrdersJob));
        var lastSync = await dbContext.Set<Sync>()
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var lastSyncTime = lastSync?.CreatedAt ?? DateTime.UtcNow.AddYears(-2);

        Log.Information("{Job}: Last sync time is {LastSyncTime}", nameof(SyncOrdersJob), lastSyncTime);

        var orderService =
            orderServiceFactory.Create(shopifyOptions.Value.ShopDomain, shopifyOptions.Value.AccessToken);
        ListFilter<Order>? filter = new OrderListFilter()
        {
            UpdatedAtMin = lastSyncTime,
            Limit = 10
        };

        int page = 0;
        while (filter is not null)
        {
            var orders = await orderService.ListAsync(filter, cancellationToken);

            if (orders is null)
            {
                Log.Error("{Job}: Failed to fetch orders from Shopify", nameof(SyncOrdersJob));
                return new MessagingError(HttpStatusCode.FailedDependency, "Failed to fetch orders from Shopify");
            }

            if (!orders.Items.Any())
            {
                Log.Information("{Job}: No orders to sync", nameof(SyncOrdersJob));
                await FinishJob(cancellationToken);
                return Result<MessagingError>.Ok();
            }

            Log.Information("{Job}: Found {OrderCount}", nameof(SyncOrdersJob), orders.Items.Count());

            foreach (var order in orders.Items)
            {
                var products = order.LineItems.Select(orderItem => new ShopifyOrderProduct
                {
                    Quantity = orderItem.Quantity!.Value,
                    ShopifyProductId = new ShopifyProductId
                    {
                        ProductId = (ulong)orderItem.ProductId!.Value,
                        VariantId = (ulong)orderItem.VariantId!.Value
                    }
                }).ToList();

                var result = await sender.Send(new CreateOrUpdateOrderCommand
                {
                    ShopifyCreatedAt = order.CreatedAt?.DateTime ?? DateTime.Now,
                    ShopifyUpdatedAt = order.UpdatedAt?.DateTime ?? DateTime.Now,
                    ShopifyCustomerId = (ulong)order.Customer.Id!.Value,
                    ShopifyOrderId = (ulong)order.Id!.Value,
                    ContactEmail = order.Email,
                    CustomerEmail = order.Customer.Email,
                    CustomerPhone = order.Customer.Phone,
                    Products = products,
                }, cancellationToken);

                if (result.IsError)
                {
                    Log.Error("{Job}: Failed to create or update order {@OrderId}", nameof(SyncOrdersJob), order.Id);
                }
                else
                {
                    Log.Information("{Job}: Synced order {OrderId}", nameof(SyncOrdersJob), order);
                }
            }

            Log.Information("{Job}: Synced page {Page} of orders", nameof(SyncOrdersJob), page);
            page++;
            filter = orders.GetNextPageFilter();
        }

        Log.Information("{Job}: Synced a total of {Page} pages", nameof(SyncOrdersJob), page);
        await FinishJob(cancellationToken);
        return Result<MessagingError>.Ok();
    }

    private async Task FinishJob(CancellationToken cancellationToken)
    {
        await dbContext.Set<Sync>().AddAsync(new Sync(), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}