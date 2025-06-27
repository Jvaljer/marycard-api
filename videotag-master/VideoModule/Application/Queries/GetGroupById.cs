using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;
using VideoModule.Models;

namespace VideoModule.Application.Queries;

public sealed record GetGroupById : IQuery<GroupModel>
{
    public required Guid GroupId { get; init; }
}

internal sealed class GetGroupByIdHandler(VideoDbContext dbContext) : IQueryHandler<GetGroupById, GroupModel>
{
    public async Task<Result<GroupModel, MessagingError>> Handle(GetGroupById request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Set<Group>()
            .AsNoTracking()
            .Include(group => group.PreviewVideo)
            .FirstOrDefaultAsync(group => group.Id == request.GroupId, cancellationToken);

        if (group is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Group not found.");
        }

        return Mapper.MapGroup(group);
    }
}