using System.Net;
using AryDotNet.Web;
using Common;
using Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoModule.Application.Commands;
using VideoModule.Application.Queries;
using VideoTag.Controllers.V2.Card;

namespace VideoTag.Controllers.V2.Video;

[Authorize(Roles = Role.Admin)]
[ApiController]
[Route("v2/video")]
public sealed class VideoController(ISender sender) : RestController(sender)
{
    [HttpPost("preview")]
    public Task<IActionResult> CreatePreviewVideo(CreateVideoRequest request, CancellationToken cancellationToken) =>
        HandleCommand(new CreatePreviewVideoCommand
        {
            VideoTitle = request.Title,
            ApiVideoId = request.ApiVideoId
        }, HttpStatusCode.Created, cancellationToken);

    [HttpGet("preview")]
    public Task<IActionResult> GetPreviewVideos([FromQuery] PageQuery pageQuery, CancellationToken cancellationToken) =>
        HandleCommand(new GetAllPreviewVideosQuery
        {
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [HttpGet("preview/{id}")]
    public Task<IActionResult> GetPreviewVideo(Guid id, CancellationToken cancellationToken) =>
        HandleCommand(new GetPreviewVideoQuery
        {
            VideoId = id
        }, cancellationToken: cancellationToken);

    [HttpPost("preview/search")]
    public Task<IActionResult> SearchPreviewVideo(SearchPreviewVideoRequest request, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) =>
        HandleCommand(new SearchPreviewVideoQuery
        {
            Query = request.Query,
            Page = pageQuery
        }, cancellationToken: cancellationToken);
}