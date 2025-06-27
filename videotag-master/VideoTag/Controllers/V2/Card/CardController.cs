using System.Net;
using AryDotNet.Web;
using Common;
using Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoModule.Application.Commands;
using VideoModule.Application.Queries;

namespace VideoTag.Controllers.V2.Card;

[Route("v2/card")]
[ApiController]
public sealed class CardController(ISender sender) : RestController(sender)
{
    [AllowAnonymous]
    [HttpGet("{identifier}")]
    public Task<IActionResult> GetCardByIdentifier(string identifier, CancellationToken cancellationToken) =>
        HandleCommand(new GetCardByIdQuery
        {
            Identifier = identifier
        }, cancellationToken: cancellationToken);


    [Authorize(Roles = Role.Admin)]
    [HttpPost("{identifier}")]
    public Task<IActionResult> CreateCard(string identifier, [FromBody] CreateCardRequest? request,
        CancellationToken cancellationToken) =>
        HandleCommand(new CreateCardCommand
        {
            Identifier = identifier,
            Url = request?.Url
        }, HttpStatusCode.Created, cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpPatch("{identifier}/url")]
    public Task<IActionResult> UpdateCardUrl(string identifier, [FromBody] UpdateCardUrlRequest request,
        CancellationToken cancellationToken) =>
        HandleCommand(new UpdateCardUrlCommand
        {
            CardIdentifier = identifier,
            Url = request.Url
        }, HttpStatusCode.NoContent, cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpPut("{identifier}/group")]
    public Task<IActionResult> UpdateCardGroup(string identifier, UpdateCardGroupRequest request,
        CancellationToken cancellationToken) =>
        HandleCommand(new UpdateCardGroupCommand
        {
            Identifier = identifier,
            GroupId = request.GroupId
        }, HttpStatusCode.NoContent, cancellationToken);

    [AllowAnonymous]
    [HttpPut("{identifier}/video")]
    public Task<IActionResult> UpdateCardVideo(string identifier, CreateVideoRequest request,
        CancellationToken cancellationToken) =>
        HandleCommand(new UpdateCardCommand
        {
            Identifier = identifier,
            VideoTitle = request.Title,
            ApiVideoId = request.ApiVideoId
        }, HttpStatusCode.NoContent, cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpPut("{identifier}/preview")]
    public Task<IActionResult> UpdatePreviewVideo(string identifier, UpdatePreviewVideoRequest request,
        CancellationToken cancellationToken) =>
        HandleCommand(new UpdateCardPreviewCommand
        {
            Identifier = identifier,
            VideoId = request.VideoId
        }, HttpStatusCode.NoContent, cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpDelete]
    [HttpDelete("{identifier}/preview")]
    public Task<IActionResult> DeletePreviewVideo(string identifier, CancellationToken cancellationToken) =>
        HandleCommand(new RemovePreviewFromCardCommand
        {
            Identifier = identifier
        }, HttpStatusCode.NoContent, cancellationToken);

    [AllowAnonymous]
    [HttpPost("{identifier}/lock")]
    public Task<IActionResult> LockCard(string identifier, CancellationToken cancellationToken) =>
        HandleCommand(new LockCardCommand
        {
            VideoIdentifier = identifier
        }, HttpStatusCode.NoContent, cancellationToken);


    [Authorize(Roles = Role.Admin)]
    [HttpPost("{identifier}/unlock")]
    public Task<IActionResult> UnlockCard(string identifier, CancellationToken cancellationToken) =>
        HandleCommand(new UnlockCardCommand
        {
            VideoIdentifier = identifier
        }, HttpStatusCode.NoContent, cancellationToken);

    [AllowAnonymous]
    [HttpDelete("{identifier}/video")]
    public Task<IActionResult> DeleteVideoFromCard(string identifier, CancellationToken cancellationToken) =>
        HandleCommand(new RemoveVideoFromCardCommand
        {
            VideoIdentifier = identifier
        }, HttpStatusCode.NoContent, cancellationToken);


    [Authorize(Roles = Role.Admin)]
    [HttpGet]
    public Task<IActionResult> GetAllCards([FromQuery] PageQuery pageQuery, CancellationToken cancellationToken) =>
        HandleCommand(new GetAllCardsQuery
        {
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [Authorize(Roles = Role.Admin)]
    [HttpPost("search")]
    public Task<IActionResult> SearchCards(SearchVideoRequest request, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) =>
        HandleCommand(new SearchCardQuery
        {
            Page = pageQuery,
            Identifier = request.Identifier,
        }, cancellationToken: cancellationToken);
}