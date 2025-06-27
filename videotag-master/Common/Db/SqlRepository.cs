using AryDotNet.Entity;
using AryDotNet.Repository;
using Microsoft.EntityFrameworkCore;

namespace Common.Db;


public abstract class SqlRepository<T, TDbContext, TEntity>(TDbContext ctx) : IRepository<TEntity, T> where TEntity : Entity<T>
    where TDbContext : DbContext
    where T : notnull
{
    public async Task<T> AddAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        var e = await ctx.AddAsync(entity, cancellationToken);
        return e.Entity.Id;
    }

    public Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = new())
    {
        return ctx.AddRangeAsync(entities, cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(T id, CancellationToken cancellationToken = new())
    {
        return ctx.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        ctx.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new())
    {
        ctx.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        entity.UpdatedAt = DateTime.Now;
        ctx.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = new())
    {
        foreach (var en in entities)
        {
            en.UpdatedAt = DateTime.Now;
        }

        ctx.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> Query()
    {
        return ctx.Set<TEntity>().AsQueryable();
    }
}

public abstract class SqlRepository<TDbContext, TEntity>(TDbContext ctx) : IRepository<TEntity> where TDbContext : DbContext where TEntity : Entity<Guid>
{
    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        var e = await ctx.AddAsync(entity, cancellationToken);
        return e.Entity.Id;
    }

    public Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = new())
    {
        return ctx.AddRangeAsync(entities, cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = new())
    {
        return ctx.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        ctx.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new())
    {
        ctx.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        entity.UpdatedAt = DateTime.Now;
        ctx.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = new())
    {
        foreach (var en in entities)
        {
            en.UpdatedAt = DateTime.Now;
        }

        ctx.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> Query()
    {
        return ctx.Set<TEntity>().AsQueryable();
    }
}