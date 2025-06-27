using System.Net;
using AnalyticsModule.Application.Commands;
using AnalyticsModule.Application.Queries;
using AryDotNet.Web;
using Common;
using Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoModule.Application.Commands;

namespace VideoTag.Controllers.V2.Analytics;

[Authorize]
[ApiController]
[Route("v2/analytics")]
public sealed class AnalyticsController(ISender sender) : RestController(sender)
{
    private readonly ISender _sender = sender;

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateActivityEventRequest request,
        CancellationToken cancellationToken)
    {
        Trusted<string>? cardId = null;
        if (request.CardId is not null)
        {
            var identifierResult = await _sender.Send(new CheckCardIdentifierCommand
            {
                Identifier = request.CardId
            }, cancellationToken);

            if (identifierResult.IsError)
            {
                return HandleResult(identifierResult);
            }

            cardId = identifierResult.Unwrap();
        }

        return await HandleCommand(new CreateActivityEventCommand
        {
            Type = request.Type,
            CardId = cardId
        }, HttpStatusCode.Created, cancellationToken: cancellationToken);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet("card/{cardId}/events")]
    public Task<IActionResult> GetActivityEventsByCardId(string cardId, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) =>
        HandleCommand(new GetCardActivityEventsQuery
        {
            Page = pageQuery,
            CardId = cardId
        }, cancellationToken: cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpGet("card/{cardId}")]
    public Task<IActionResult> GetActivityByCardId(string cardId, CancellationToken cancellationToken) =>
        HandleCommand(new GetCardActivityQuery
        {
            CardId = cardId
        }, cancellationToken: cancellationToken);

    [HttpGet("orders/{orderId}")]
    public Task<IActionResult> GetCardStatsByOrderId(Guid orderId, CancellationToken cancellationToken) =>
        HandleCommand(new GetOrderStatsQuery()
        {
            OrderId = orderId
        }, cancellationToken: cancellationToken);
}