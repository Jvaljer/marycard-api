using AryDotNet.Web;
using Common;
using Common.Paging;
using JobModule.Application.Queries;
using JobModule.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoTag.Controllers.V2.Job;

[Authorize(Roles = Role.Admin)]
[ApiController]
[Route("v2/job")]
internal sealed class JobController(ISender sender) : RestController(sender)
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetJobById(Guid id, CancellationToken cancellationToken) => HandleCommand(
        new GetJobByIdQuery
        {
            JobId = id
        }, cancellationToken: cancellationToken);

    [HttpGet("recent")]
    public Task<IActionResult> GetRecentJobs([FromQuery] PageQuery pageQuery, [FromQuery] JobStatus? status, CancellationToken cancellationToken) => HandleCommand(
        new GetJobsQuery
        {
            Page = pageQuery,
            Status = status
        }, cancellationToken: cancellationToken);
}