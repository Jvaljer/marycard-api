using System.Net;
using AryDotNet.Web;
using Common;
using Common.Models;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Application.Queries;
using InventoryModule.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderModule.Application.Commands;
using OrderModule.Application.Queries;

namespace VideoTag.Controllers.V2.Order;

[ApiController]
[Route("v2/order")]
[Authorize(Roles = Role.Admin)]
public sealed class OrderController(ISender sender) : RestController(sender)
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) => HandleCommand(
        new GetOrderById
        {
            OrderId = id
        }, cancellationToken: cancellationToken);

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new CreateFakeOrderCommand
        {
            ContactEmail = request.ContactEmail,
            CustomerEmail = request.CustomerEmail,
            Note = request.Note,
            CustomerPhone = request.CustomerPhone,
            CustomerFirstName = request.CustomerFirstName,
            CustomerLastName = request.CustomerLastName
        }, cancellationToken);

        return HandleResult(result.MapValue(v => new EntityId<Guid>(v)), HttpStatusCode.Created);
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, UpdateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new UpdateOrderCommand
        {
            OrderId = orderId,
            Note = request.Note,
            State = request.State,
            CustomerPhone = request.CustomerPhone,
            CustomerFirstName = request.CustomerFirstName,
            CustomerLastName = request.CustomerLastName,
            CustomerEmail = request.CustomerEmail,
            ContactEmail = request.ContactEmail
        }, cancellationToken);

        return HandleResult(result, HttpStatusCode.NoContent);
    }

    [HttpGet("shopify/{shopifyId}")]
    public Task<IActionResult> GetByShopifyId(ulong shopifyId, CancellationToken cancellationToken) => HandleCommand(
        new GetOrderByShopifyId
        {
            ShopifyOrderId = shopifyId
        }, cancellationToken: cancellationToken);

    [HttpGet("item/{itemId}")]
    public Task<IActionResult> GetByItemId(Guid itemId, CancellationToken cancellationToken) => HandleCommand(
        new GetOrderByItemId
        {
            ItemId = itemId
        }, cancellationToken: cancellationToken);

    [HttpGet("card/{physicalCardId}")]
    public Task<IActionResult> GetByCardId(Guid physicalCardId, CancellationToken cancellationToken) => HandleCommand(
        new GetOrderByPhysicalCardIdQuery
        {
            PhysicalCardId = physicalCardId
        }, cancellationToken: cancellationToken);

    [HttpGet("customer/{customerId}")]
    public Task<IActionResult> GetByCustomerId(ulong customerId, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) => HandleCommand(
        new GetOrdersByCustomerId
        {
            CustomerId = customerId,
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [HttpGet("recent")]
    public Task<IActionResult> GetRecent([FromQuery] PageQuery pageQuery, [FromQuery] OrderState? state,
        CancellationToken cancellationToken) =>
        HandleCommand(
            new GetRecentOrders
            {
                State = state,
                Page = pageQuery
            }, cancellationToken: cancellationToken);

    [HttpPost("search")]
    public Task<IActionResult> SearchOrders([FromBody] SearchOrderRequest request, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) => HandleCommand(
        new SearchOrders
        {
            Text = request.Text,
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [HttpPost("products/{orderProductId}/item")]
    public async Task<IActionResult> CreateOrderItem(Guid orderProductId, [FromBody] CreateOrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var physicalProductResult = await Sender.Send(new GetPhysicalCardById
        {
            Id = request.PhysicalCardId
        }, cancellationToken);

        if (physicalProductResult.IsError)
        {
            return HandleResult(physicalProductResult);
        }

        var creationResult = await Sender.Send(new CreateOrderItemCommand
        {
            OrderProductId = orderProductId,
            PhysicalCard = Trusted<PhysicalCardModel>.From(physicalProductResult.Unwrap())
        }, cancellationToken);
        return HandleResult(creationResult, HttpStatusCode.Created);
    }

    [HttpDelete("{orderId}/item/{physicalCardId}")]
    public Task<IActionResult>
        DeleteOrderItem(Guid orderId, Guid physicalCardId, CancellationToken cancellationToken) =>
        HandleCommand(
            new DeleteOrderItemCommand
            {
                OrderId = orderId,
                PhysicalCardId = physicalCardId
            }, cancellationToken: cancellationToken);

    [HttpPatch("{orderId}/state")]
    public Task<IActionResult> UpdateOrderState(Guid orderId, [FromBody] UpdateOrderStateRequest request,
        CancellationToken cancellationToken) => HandleCommand(new UpdateOrderStateCommand
        {
            State = request.OrderState,
            OrderId = orderId
        }, cancellationToken: cancellationToken);
}