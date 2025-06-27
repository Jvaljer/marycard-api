using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record UpdateGroupCommand : ICommand
{
    public required Guid GroupId { get; init; }
    public required string Name { get; init; }
    public required Guid? PreviewVideoId { get; init; }
}

internal sealed class UpdateGroupHandler(VideoDbContext dbContext) : ICommandHandler<UpdateGroupCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Set<Group>()
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (group is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Group not found.");
        }

        if (request.PreviewVideoId is not null)
        {
            var previewVideoExists = await dbContext.Set<PreviewVideo>()
                .AnyAsync(v => v.Id == request.PreviewVideoId, cancellationToken);

            if (!previewVideoExists)
            {
                return new MessagingError(HttpStatusCode.NotFound, "Preview video not found.");
            }
        }

        group.Name = request.Name;
        group.PreviewVideoId = request.PreviewVideoId;
        dbContext.Set<Group>().Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}