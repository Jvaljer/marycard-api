using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Worker.Events;
using JobModule.Domain;
using JobModule.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JobModule.Application.Events;

internal sealed class JobFailedEventHandler(JobDbContext jobDbContext, IRepository<Job> jobRepository) : IEventHandler<JobFailedEvent>
{
    public async Task Handle(JobFailedEvent notification, CancellationToken cancellationToken)
    {

        var job = await jobRepository.Query().FirstOrDefaultAsync(job => job.Id == notification.JobId, cancellationToken);

        if (job is null)
        {
            await jobRepository.AddAsync(new Job
            {
                Status = JobStatus.Failed,
                Id = notification.JobId,
                Name = notification.JobName,
                FailureCode = (int)notification.Error.Code,
                FailureReason = notification.Error.Message
            }, cancellationToken);
        }
        else
        {
            job.Status = JobStatus.Failed;
            job.FailureCode = (int)notification.Error.Code;
            job.FailureReason = notification.Error.Message;
            await jobRepository.UpdateAsync(job, cancellationToken);
        }

        await jobDbContext.SaveChangesAsync(cancellationToken);
    }
}