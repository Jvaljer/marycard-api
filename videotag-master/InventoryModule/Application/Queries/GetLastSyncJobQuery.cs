using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Repository;
using AryDotNet.Result;
using InventoryModule.Domain.Entities;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetLastSyncJobQuery : IQuery<SyncJobModel>;

internal sealed class GetLastSyncJobHandler(IRepository<SyncJob> jobRepository) : IQueryHandler<GetLastSyncJobQuery, SyncJobModel>
{
    public async Task<Result<SyncJobModel, MessagingError>> Handle(GetLastSyncJobQuery request, CancellationToken cancellationToken)
    {
        var job = await jobRepository.Query().AsNoTracking()
            .OrderByDescending(j => j.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "No sync job found");
        }

        return Result<SyncJobModel, MessagingError>.Ok(new SyncJobModel
        {
            Status = job.Status,
            CreatedAt = job.CreatedAt
        });
    }
}