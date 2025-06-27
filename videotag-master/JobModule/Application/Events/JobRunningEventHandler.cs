using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Worker.Events;
using JobModule.Domain;
using JobModule.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JobModule.Application.Events;

internal sealed class JobRunningEventHandler(JobDbContext jobDbContext, IRepository<Job> jobRepository) : IEventHandler<JobRunningEvent>
{
    public async Task Handle(JobRunningEvent notification, CancellationToken cancellationToken)
    {
        var job = await jobRepository.Query().FirstOrDefaultAsync(job => job.Id == notification.JobId, cancellationToken);

        if (job is null)
        {
            await jobRepository.AddAsync(new Job
            {
                Name = notification.JobName,
                Status = JobStatus.InProgress,
                Id = notification.JobId,
            }, cancellationToken);
        }
        else
        {
            job.Status = JobStatus.InProgress;
            await jobRepository.UpdateAsync(job, cancellationToken);
        }

        await jobDbContext.SaveChangesAsync(cancellationToken);
    }
}