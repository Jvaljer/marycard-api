using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using Common.Paging;
using JobModule.Domain;
using JobModule.Models;
using Microsoft.EntityFrameworkCore;

namespace JobModule.Application.Queries;

public sealed record GetJobsQuery : IQuery<ICollection<JobModel>>
{
    public required PageQuery Page { get; init; }
    public JobStatus? Status { get; init; }
}

internal sealed class GetJobsHandler(IRepository<Job> jobRepository) : IQueryHandler<GetJobsQuery, ICollection<JobModel>>
{
    public async Task<Result<ICollection<JobModel>, MessagingError>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var dbRequest = jobRepository.Query().AsNoTracking()
            .PagedOrderedDescending(request.Page);

        if (request.Status is not null)
        {
            dbRequest = dbRequest.Where(job => job.Status == request.Status);
        }

        return await dbRequest
            .Select(job => Mapper.JobToJobModel(job))
            .ToListAsync(cancellationToken);
    }
}