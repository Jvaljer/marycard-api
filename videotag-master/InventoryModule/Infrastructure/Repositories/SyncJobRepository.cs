using Common.Db;
using InventoryModule.Domain.Entities;

namespace InventoryModule.Infrastructure.Repositories;

internal sealed class SyncJobRepository(InventoryDbContext ctx) : SqlRepository<InventoryDbContext, SyncJob>(ctx);