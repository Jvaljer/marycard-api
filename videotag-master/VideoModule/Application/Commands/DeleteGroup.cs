using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record DeleteGroup : ICommand
{
    public required Guid GroupId { get; init; }
}

internal sealed class DeleteGroupHandler(VideoDbContext dbContext) : ICommandHandler<DeleteGroup>
{
    public async Task<Result<MessagingError>> Handle(DeleteGroup request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Set<Group>()
            .FirstOrDefaultAsync(v => v.Id == request.GroupId, cancellationToken);

        if (group is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Group not found");
        }

        dbContext.Set<Group>().Remove(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<MessagingError>.Ok();
    }
}