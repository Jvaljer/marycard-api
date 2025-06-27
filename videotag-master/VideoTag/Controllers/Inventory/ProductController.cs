using AryDotNet.Web;
using Common;
using Common.Paging;
using Common.Shopify;
using InventoryModule.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoTag.Controllers.Inventory;

[Route("v1/product")]
[Authorize(Roles = Role.Admin)]
[ApiController]
public sealed class ProductController(ISender sender) : RestController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetProducts([FromQuery] PageQuery pageQuery, CancellationToken cancellationToken) =>
        HandleCommand(new GetProductsQuery
        {
            Page = pageQuery,
        }, cancellationToken: cancellationToken);

    [HttpGet("{id}/variant/{variantId}")]
    public Task<IActionResult> GetByShopifyId(ulong id, ulong variantId, CancellationToken cancellationToken) =>
        HandleCommand(new GetProductByIdQuery()
        {
            ShopifyProductId = new ShopifyProductId
            {
                ProductId = id,
                VariantId = variantId
            }
        }, cancellationToken: cancellationToken);

    [HttpGet("status/sync")]
    public Task<IActionResult> GetLastSyncJob(CancellationToken cancellationToken) =>
        HandleCommand(new GetLastSyncJobQuery(), cancellationToken: cancellationToken);

    [HttpGet("{id}/variant/{variantId}/illustrations")]
    public Task<IActionResult> GetIllustrationsByShopifyProductId(ulong id, ulong variantId,
        CancellationToken cancellationToken)
        => HandleCommand(new GetIllustrationListByShopifyProductId
        {
            ShopifyProductId = new ShopifyProductId
            {
                ProductId = id,
                VariantId = variantId
            }
        }, cancellationToken: cancellationToken);
}