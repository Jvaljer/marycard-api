using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Worker.Events;
using JobModule.Domain;
using JobModule.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JobModule.Application.Events;

internal sealed class JobFinishedEventHandler(JobDbContext jobDbContext, IRepository<Job> jobRepository) : IEventHandler<JobFinishedEvent>
{
    public async Task Handle(JobFinishedEvent notification, CancellationToken cancellationToken)
    {
        var job = await jobRepository.Query().FirstOrDefaultAsync(job => job.Id == notification.JobId, cancellationToken);

        if (job is null)
        {
            await jobRepository.AddAsync(new Job
            {
                Name = notification.JobName,
                Status = JobStatus.Completed,
                Id = notification.JobId,
            }, cancellationToken);
        }
        else
        {
            job.Status = JobStatus.Completed;
            await jobRepository.UpdateAsync(job, cancellationToken);
        }

        await jobDbContext.SaveChangesAsync(cancellationToken);
    }
}