using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Worker.Events;
using JobModule.Domain;
using JobModule.Infrastructure;

namespace JobModule.Application.Events;

internal sealed class JobCreatedEventHandler(JobDbContext jobDbContext, IRepository<Job> jobRepository) : IEventHandler<JobCreatedEvent>
{
    public async Task Handle(JobCreatedEvent notification, CancellationToken cancellationToken)
    {
        await jobRepository.AddAsync(new Job
        {
            Id = notification.JobId,
            Name = notification.JobName,
            Status = JobStatus.Created,
        }, cancellationToken);
        await jobDbContext.SaveChangesAsync(cancellationToken);
    }
}