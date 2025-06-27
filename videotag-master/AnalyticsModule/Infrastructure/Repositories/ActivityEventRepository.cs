using AnalyticsModule.Domain;
using Common.Db;

namespace AnalyticsModule.Infrastructure.Repositories;

internal sealed class ActivityEventRepository(AnalyticsDbContext ctx) : SqlRepository<AnalyticsDbContext, ActivityEvent>(ctx);