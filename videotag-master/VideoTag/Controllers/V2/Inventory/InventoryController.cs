using System.Net;
using AryDotNet.Web;
using Common;
using Common.Models;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Application.Commands;
using InventoryModule.Application.Queries;
using InventoryModule.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoTag.Controllers.V2.Inventory;

[Authorize(Roles = Role.Admin)]
[ApiController]
[Route("v2/[controller]")]
public sealed class InventoryController(ISender sender) : RestController(sender)
{
    /// <summary>
    /// Get a physical card
    /// </summary>
    /// <param name="id">Physical card ID or tag ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType<PhysicalCardModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("cards/{id}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) => HandleCommand(
        new GetPhysicalCardById
        {
            Id = id
        }, cancellationToken: cancellationToken);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("cards/{physicalCardId}")]
    public Task<IActionResult> UpdatePhysicalCard(Guid physicalCardId, UpdatePhysicalCardRequest request,
        CancellationToken cancellationToken)
        => HandleCommand(new UpdatePhysicalCard
        {
            Note = request.Note,
            CountryCodeWarehouse = request.WarehouseCountryCode,
            PhysicalCardId = physicalCardId,
            IllustrationId = request.IllustrationId
        }, HttpStatusCode.NoContent, cancellationToken: cancellationToken);

    [HttpPost("cards/{physicalCardId}/redirection")]
    public Task<IActionResult> UpdatePhysicalCardRedirection(Guid physicalCardId,
        UpdatePhysicalCardRedirectionRequest request,
        CancellationToken cancellationToken)
        => HandleCommand(new UpdatePhysicalCardRedirection
        {
            PhysicalCardId = physicalCardId,
            Url = request.Url
        }, HttpStatusCode.NoContent, cancellationToken: cancellationToken);

    [HttpGet("cards/{physicalCardId}/redirection")]
    public Task<IActionResult> GetPhysicalCardRedirection(Guid physicalCardId, CancellationToken cancellationToken)
        => HandleCommand(new GetPhysicalCardRedirectionQuery
        {
            PhysicalCardId = physicalCardId
        }, cancellationToken: cancellationToken);

    [HttpDelete("cards/{physicalCardId}/redirection")]
    public Task<IActionResult> DeletePhysicalCardRedirection(Guid physicalCardId, CancellationToken cancellationToken)
        => HandleCommand(new UpdatePhysicalCardRedirection
        {
            PhysicalCardId = physicalCardId,
            Url = null
        }, HttpStatusCode.NoContent, cancellationToken: cancellationToken);

    [ProducesResponseType<EntityId<Guid>>(StatusCodes.Status201Created)]
    [HttpPost("cards")]
    public Task<IActionResult> CreateCard([FromBody] CreatePhysicalCard command,
        CancellationToken cancellationToken) => HandleCommand(command, successCode: HttpStatusCode.Created,
        cancellationToken: cancellationToken);

    [ProducesResponseType<IReadOnlyList<PhysicalCardModel>>(StatusCodes.Status200OK)]
    [HttpGet("cards")]
    public Task<IActionResult> GetRecentCards([FromQuery] string? videoCardId, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) => HandleCommand(
        new GetPhysicalCardList()
        {
            Page = pageQuery,
            CardId = videoCardId
        }, cancellationToken: cancellationToken);

    [ProducesResponseType<IllustrationModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("illustrations/{id}")]
    public Task<IActionResult> GetIllustration(Guid id, CancellationToken cancellationToken) => HandleCommand(
        new GetIllustrationById
        {
            IllustrationId = id
        }, cancellationToken: cancellationToken);

    [ProducesResponseType<EntityId<Guid>>(StatusCodes.Status201Created)]
    [HttpPost("illustrations")]
    public Task<IActionResult> CreateIllustration([FromBody] CreateIllustration command,
        CancellationToken cancellationToken) => HandleCommand(command, successCode: HttpStatusCode.Created,
        cancellationToken: cancellationToken);

    [ProducesResponseType<IReadOnlyList<IllustrationModel>>(StatusCodes.Status201Created)]
    [HttpGet("illustrations")]
    public Task<IActionResult> GetRecentIllustrations([FromQuery] string? name, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) => HandleCommand(
        new GetIllustrationList()
        {
            Page = pageQuery,
            Name = name
        }, cancellationToken: cancellationToken);

    [ProducesResponseType<IReadOnlyList<PhysicalCardModel>>(StatusCodes.Status200OK)]
    [HttpGet("illustrations/{id}/cards")]
    public Task<IActionResult> GetCardsByIllustration(Guid id, [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken) => HandleCommand(
        new GetPhysicalCardListByIllustrationId()
        {
            IllustrationId = id,
            Page = pageQuery
        }, cancellationToken: cancellationToken);
}