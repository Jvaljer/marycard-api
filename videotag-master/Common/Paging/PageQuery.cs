using System.Linq.Expressions;
using AryDotNet.Entity;

namespace Common.Paging;

public sealed record PageQuery()
{
    public required uint Page { get; init; }
    public required DateTime Until { get; init; }

    private uint _pageSize = Constants.DefaultPageSize;
    public int Skip => (int)(Page * _pageSize);
    public int Take => (int)_pageSize;

    public int Size => (int)_pageSize;

    public PageQuery WithPageSize(uint pageSize)
    {
        _pageSize = pageSize;
        return this;
    }

    public PageQuery Increment() => new()
    {
        Page = Page + 1,
        Until = Until,
        _pageSize = _pageSize
    };
}

public static class EntityExtension
{
    public static IQueryable<T> PagedOrderedDescending<T>(this IQueryable<T> query, PageQuery Page) where T : Entity<Guid>
    {
        return query
            .Where(e => e.CreatedAt <= Page.Until)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(Page.Skip)
            .Take(Page.Take);
    }

    public static IQueryable<TSource> PagedOrderedDescending<TSource, TKey>(this IQueryable<TSource> query, PageQuery Page, Expression<Func<TSource, TKey>> keySelector) where TSource : Entity<Guid>
    {
        return query
            .Where(e => e.CreatedAt <= Page.Until)
            .OrderByDescending(keySelector)
            .Skip(Page.Skip)
            .Take(Page.Take);
    }

    public static IQueryable<TSource> PagedOrderedDescending<TSource>(
        this IQueryable<TSource> query,
        PageQuery page,
        Expression<Func<TSource, DateTimeOffset>> dateSelector)
    {
        // Build the filtering predicate: dateSelector(e) <= page.Until
        var parameter = dateSelector.Parameters[0];
        var untilConstant = Expression.Constant((DateTimeOffset)page.Until.ToUniversalTime(), typeof(DateTimeOffset));
        var filterBody = Expression.LessThanOrEqual(dateSelector.Body, untilConstant);
        var filterPredicate = Expression.Lambda<Func<TSource, bool>>(filterBody, parameter);

        return query
            .Where(filterPredicate)
            .OrderByDescending(dateSelector)
            .Skip(page.Skip)
            .Take(page.Take);
    }

    public static IQueryable<T> PagedOrdered<T>(this IQueryable<T> query, PageQuery Page) where T : Entity<Guid>
    {
        return query
            .Where(e => e.CreatedAt <= Page.Until)
            .OrderBy(e => e.CreatedAt)
            .Skip(Page.Skip)
            .Take(Page.Take);
    }

    public static IQueryable<TSource> PagedOrdered<TSource, TKey>(this IQueryable<TSource> query, PageQuery Page, Expression<Func<TSource, TKey>> keySelector) where TSource : Entity<Guid>
    {
        return query
            .Where(e => e.CreatedAt <= Page.Until)
            .OrderBy(keySelector)
            .Skip(Page.Skip)
            .Take(Page.Take);
    }

    public static IQueryable<TSource> Paged<TSource>(this IQueryable<TSource> query, PageQuery page)
        where TSource : Entity<Guid>
    {
        return query
            .Where(e => e.CreatedAt <= page.Until)
            .Skip(page.Skip)
            .Take(page.Take);
    }
}
