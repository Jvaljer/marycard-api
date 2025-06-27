using System.Net;
using Common;
using Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoModule.Application.Commands;
using VideoModule.Application.Queries;
using VideoTag.Controllers.V1.Models;
using VideoTag.Controllers.V2.Card;

namespace VideoTag.Controllers.V1.Video;

[Route("v1/video")]
[ApiController]
public sealed class VideoController(ISender sender) : OldRestController
{
    [AllowAnonymous]
    [HttpGet("{identifier}")]
    public async Task<IActionResult> GetVideoByIdentifier(string identifier, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCardByIdQuery()
        {
            Identifier = identifier
        }, cancellationToken);
        return HandleResult(result.MapValue(OldVideoModel.FromCard));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateVideo(OldCreateVideoRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCardCommand
        {
            Identifier = request.Identifier,
            VideoTitle = request.Title,
            ApiVideoId = request.ApiVideoId
        }, cancellationToken);
        var r = await sender.Send(new GetCardByIdQuery
        {
            Identifier = request.Identifier
        }, cancellationToken);
        return HandleResult(r.MapValue(OldVideoModel.FromCard), HttpStatusCode.Created);
    }

    [AllowAnonymous]
    [HttpPut("{identifier}")]
    public async Task<IActionResult> UpdateVideo(string identifier, OldCreateVideoRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCardCommand
        {
            ApiVideoId = request.ApiVideoId,
            Identifier = identifier,
            VideoTitle = request.Title
        }, cancellationToken);
        return HandleResult(result, HttpStatusCode.OK);
    }

    [AllowAnonymous]
    [HttpPost("{identifier}/lock")]
    public async Task<IActionResult> LockVideo(string identifier, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LockCardCommand
        {
            VideoIdentifier = identifier
        }, cancellationToken);
        return HandleResult(result);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost("{identifier}/unlock")]
    public async Task<IActionResult> UnlockVideo(string identifier, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UnlockCardCommand()
        {
            VideoIdentifier = identifier
        }, cancellationToken);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpDelete("{identifier}")]
    public async Task<IActionResult> DeleteVideo(string identifier, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveVideoFromCardCommand
        {
            VideoIdentifier = identifier
        }, cancellationToken);
        return HandleResult(result);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAllVideos([FromQuery] PageQuery pageQuery, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllCardsQuery()
        {
            Page = pageQuery
        }, cancellationToken);
        return HandleResult(result.MapValue((cards) =>
            cards.Select(OldVideoModel.FromCard).Where(v => v is not null).ToList()));
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost("search")]
    public async Task<IActionResult> SearchVideos(SearchVideoRequest request, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SearchCardQuery()
        {
            Page = pageQuery,
            Identifier = request.Identifier,
        }, cancellationToken);
        return HandleResult(result.MapValue((cards) =>
            cards.Select(OldVideoModel.FromCard).Where(v => v is not null).ToList()));
    }
}