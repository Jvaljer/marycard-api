using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using JobModule.Domain;
using JobModule.Models;
using Microsoft.EntityFrameworkCore;

namespace JobModule.Application.Queries;

public sealed record GetJobByIdQuery : IQuery<JobModel>
{
    public required Guid JobId { get; init; }
}

internal sealed class GetJobByIdQueryHandler(IRepository<Job> jobRepository) : IQueryHandler<GetJobByIdQuery, JobModel>
{

    public async Task<Result<JobModel, MessagingError>> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await jobRepository.Query().AsNoTracking().FirstOrDefaultAsync(job => job.Id == request.JobId, cancellationToken);

        if (job is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Job not found");
        }

        return Mapper.JobToJobModel(job);
    }
}