using Common.Db;
using JobModule.Domain;

namespace JobModule.Infrastructure.Repositories;

internal sealed class JobRepository(JobDbContext ctx) : SqlRepository<JobDbContext, Job>(ctx);