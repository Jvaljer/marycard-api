using System.Net;
using AryDotNet.Web;
using Common;
using Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoModule.Application.Commands;
using VideoModule.Application.Queries;

namespace VideoTag.Controllers.V2.Group;

[Authorize(Roles = Role.Admin)]
[ApiController]
[Route("v2/groups")]
public sealed class GroupController(ISender sender) : RestController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetGroupList([FromQuery] string? name, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) =>
        HandleCommand(new GetGroupList
        {
            Name = name,
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [HttpGet("{groupId}")]
    public Task<IActionResult> GetGroupById(Guid groupId, CancellationToken cancellationToken) =>
        HandleCommand(new GetGroupById
        {
            GroupId = groupId
        }, cancellationToken: cancellationToken);

    [HttpGet("{groupId}/cards")]
    public Task<IActionResult> GetCardListByGroupId(Guid groupId, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) =>
        HandleCommand(new GetCardListByGroupId
        {
            GroupId = groupId,
            Page = pageQuery
        }, cancellationToken: cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateGroup(CreateGroupRequest request, CancellationToken cancellationToken) =>
        HandleCommand(new CreateGroup
        {
            Name = request.Name,
            PreviewVideoId = request.PreviewVideoId
        }, HttpStatusCode.Created, cancellationToken);

    [HttpDelete("{groupId}")]
    public Task<IActionResult> DeleteGroup(Guid groupId, CancellationToken cancellationToken) =>
        HandleCommand(new DeleteGroup
        {
            GroupId = groupId
        }, HttpStatusCode.NoContent, cancellationToken);

    [HttpPut("{groupId}")]
    public Task<IActionResult> UpdateGroup(Guid groupId, UpdateGroupRequest request,
        CancellationToken cancellationToken) => HandleCommand(new UpdateGroupCommand
        {
            Name = request.Name,
            GroupId = groupId,
            PreviewVideoId = request.PreviewVideoId
        }, HttpStatusCode.NoContent, cancellationToken: cancellationToken);
}