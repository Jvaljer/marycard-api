using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Web;
using Common.Shopify;
using InventoryApi.Models;
using InventoryModule.Application.Commands;
using InventoryModule.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderModule.Application.Commands;
using VideoTag.Controllers.Sales.ShopifyModels;
using VideoTag.Middlewares;

namespace VideoTag.Controllers.Sales;

[Route("v1/sales/webhooks")]
[Authorize(AuthenticationSchemes = ShopifyAuthenticationHandler.SchemeName)]
[ApiController]
public sealed class ShopifyWebhooksController(ISender sender) : RestController(sender)
{
    private readonly ISender _sender = sender;

    [HttpPost("products/created")]
    public async Task<IActionResult> ProductCreated([FromBody] ShopifyProduct product,
        CancellationToken cancellationToken)
    {
        if (product.Variants is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "Variants are required"
            ));
        }

        var result = await _sender.Send(new CreateOrUpdateProductsCommand
        {
            Products = product.Variants.Select(variant => new ProductContentModel
            {
                ShopifyProductId = new ShopifyProductId
                {
                    ProductId = (ulong)product.Id!,
                    VariantId = (ulong)variant.Id!
                },
                SKU = variant.SKU,
                Name = product.Title,
                VariantName = variant.Title,
                Description = product.BodyHtml ?? "",
            }).ToList()
        }, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("products/updated")]
    public async Task<IActionResult> ProductUpdated([FromBody] ShopifyProduct product,
        CancellationToken cancellationToken)
    {
        if (product.Variants is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "Variants are required"
            ));
        }

        var result = await _sender.Send(new CreateOrUpdateProductsCommand
        {
            Products = product.Variants.Select(variant => new ProductContentModel
            {
                ShopifyProductId = new ShopifyProductId
                {
                    ProductId = (ulong)product.Id!,
                    VariantId = (ulong)variant.Id!
                },
                SKU = variant.SKU,
                Name = product.Title,
                VariantName = variant.Title,
                Description = product.BodyHtml ?? "",
            }).ToList()
        }, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("products/deleted")]
    public async Task<IActionResult> ProductDeleted([FromBody] DeleteRequest product,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteVariantsCommand { ProductId = product.Id }, cancellationToken);
        return Ok(HttpStatusCode.NoContent);
    }

    [HttpPost("orders/created")]
    public async Task<IActionResult> OrderCreated([FromBody] ShopifyOrder order, CancellationToken cancellationToken)
    {
        if (order.Customer is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "Customer is required"
            ));
        }

        if (order.LineItems is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "LineItems are required"
            ));
        }

        var products = order.LineItems.Select(orderItem => new ShopifyOrderProduct
        {
            Quantity = orderItem.Quantity!.Value,
            ShopifyProductId = new ShopifyProductId
            {
                ProductId = (ulong)orderItem.ProductId!.Value,
                VariantId = (ulong)orderItem.VariantId!.Value
            }
        }).ToList();

        var result = await _sender.Send(new CreateOrUpdateOrderCommand
        {
            ShopifyCreatedAt = order.CreatedAt?.DateTime ?? DateTime.Now,
            ShopifyUpdatedAt = order.UpdatedAt?.DateTime ?? DateTime.Now,
            ShopifyCustomerId = (ulong)order.Customer.Id!.Value,
            CustomerPhone = order.Customer.Phone,
            CustomerEmail = order.Customer.Email,
            ContactEmail = order.Email,
            CustomerFirstName = order.Customer.FirstName,
            CustomerLastName = order.Customer.LastName,
            ShopifyOrderId = (ulong)order.Id!.Value,
            Products = products,
        }, cancellationToken);

        return HandleResult(result, HttpStatusCode.Created);
    }

    [HttpPost("orders/updated")]
    public async Task<IActionResult> OrderUpdated([FromBody] ShopifyOrder order, CancellationToken cancellationToken)
    {
        if (order.Customer is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "Customer is required"
            ));
        }

        if (order.LineItems is null)
        {
            return HandleError(new MessagingError
            (
                HttpStatusCode.BadRequest,
                "LineItems are required"
            ));
        }

        var products = order.LineItems.Select(orderItem => new ShopifyOrderProduct
        {
            Quantity = orderItem.Quantity!.Value,
            ShopifyProductId = new ShopifyProductId
            {
                ProductId = (ulong)orderItem.ProductId!.Value,
                VariantId = (ulong)orderItem.VariantId!.Value
            }
        }).ToList();

        var result = await _sender.Send(new CreateOrUpdateOrderCommand
        {
            ShopifyCreatedAt = order.CreatedAt?.DateTime ?? DateTime.Now,
            ShopifyUpdatedAt = order.UpdatedAt?.DateTime ?? DateTime.Now,
            ShopifyCustomerId = (ulong)order.Customer.Id!.Value,
            ShopifyOrderId = (ulong)order.Id!.Value,
            CustomerPhone = order.Customer.Phone,
            CustomerEmail = order.Customer.Email,
            CustomerFirstName = order.Customer.FirstName,
            CustomerLastName = order.Customer.LastName,
            ContactEmail = order.Email,
            Products = products,
        }, cancellationToken);

        return HandleResult(result);
    }
}