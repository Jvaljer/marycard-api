using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed record GetGroupList : IQuery<IReadOnlyList<GroupModel>>
{
    public required string? Name { get; init; }
    public required PageQuery Page { get; init; }
}

internal sealed class GetGroupListHandler(VideoDbContext dbContext)
    : IQueryHandler<GetGroupList, IReadOnlyList<GroupModel>>
{
    public async Task<Result<IReadOnlyList<GroupModel>, MessagingError>> Handle(GetGroupList request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Group>().AsNoTracking();

        if (request.Name is not null)
        {
            var normalizedName = request.Name.ToUpperInvariant();
            query = query.Where(group => group.NormalizedName.Contains(normalizedName));
        }

        var groups = await query.PagedOrderedDescending(request.Page, group => group.CreatedAt)
            .Include(group => group.PreviewVideo)
            .ToListAsync(cancellationToken);

        return groups.Select(Mapper.MapGroup).ToList();
    }
}