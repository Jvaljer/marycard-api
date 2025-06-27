using System.Net;
using AnalyticsModule.Application.Commands;
using AryDotNet.Messaging;
using AryDotNet.Web;
using Common;
using InventoryModule.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TagClient.Client;

namespace VideoTag.Controllers.V2.Tag;

[Authorize(Roles = Role.Admin)]
[ApiController]
[Route("v2/tag")]
public sealed class TagController(ISender sender, ITagClient tagClient) : RestController(sender)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(Guid id, CancellationToken cancellationToken)
    {
        var tag = await tagClient.GetById(id, cancellationToken);
        return HandleResult(tag.MapError((e) =>
            new MessagingError((HttpStatusCode)e.Code, e.Message ?? "An error occurred")));
    }

    [HttpGet("batch/{batchName}/tag/{tagUid}")]
    public async Task<IActionResult> GetTag(string batchName, string tagUid, CancellationToken cancellationToken)
    {
        var tag = await tagClient.GetByUid(tagUid, batchName, cancellationToken);
        return HandleResult(tag.MapError((e) =>
            new MessagingError((HttpStatusCode)e.Code, e.Message ?? "An error occurred")));
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> HandleTagRedirected([FromBody] TagRedirectedModel model,
        CancellationToken cancellationToken)
    {
        var physicalCard = await Sender.Send(new GetPhysicalCardById
        {
            Id = model.Tag.Id
        }, cancellationToken);

        if (physicalCard.IsError)
        {
            return HandleResult(physicalCard);
        }

        return await HandleCommand(new SetTagRedirectedCount
        {
            PhysicalCardId = physicalCard.Unwrap().Id,
            TagRedirectedCount = model.Tag.UsageCounter
        }, HttpStatusCode.Created, cancellationToken: cancellationToken);
    }
}