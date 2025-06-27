using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using VideoModule.Domain.Entities;
using VideoModule.Infrastructure;

namespace VideoModule.Application.Commands;

public sealed record CreateGroup : ICommand<EntityId<Guid>>
{
    public required string Name { get; init; }
    public Guid? PreviewVideoId { get; init; }
}

internal sealed class CreateGroupHandler(VideoDbContext dbContext) : ICommandHandler<CreateGroup, EntityId<Guid>>
{
    public async Task<Result<EntityId<Guid>, MessagingError>> Handle(CreateGroup request,
        CancellationToken cancellationToken)
    {
        if (request.PreviewVideoId is not null)
        {
            var videoExists = await dbContext.Set<PreviewVideo>()
                .AnyAsync(v => v.Id == request.PreviewVideoId, cancellationToken);

            if (!videoExists)
            {
                return new MessagingError(HttpStatusCode.NotFound, "Preview video not found.");
            }
        }

        var entityEntryId = await dbContext.Set<Group>().AddAsync(new Group
        {
            Name = request.Name,
            NormalizedName = request.Name.ToUpperInvariant(),
            PreviewVideoId = request.PreviewVideoId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EntityId<Guid>(entityEntryId.Entity.Id);
    }
}