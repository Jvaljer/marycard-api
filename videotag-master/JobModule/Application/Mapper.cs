using JobModule.Domain;
using JobModule.Models;

namespace JobModule.Application;

internal static class Mapper
{
    public static JobModel JobToJobModel(Job job) => new()
    {
        Id = job.Id,
        Status = job.Status,
        FailureReason = job.FailureReason,
        FailureCode = job.FailureCode,
        CreatedAt = job.CreatedAt,
        UpdatedAt = job.UpdatedAt
    };
}