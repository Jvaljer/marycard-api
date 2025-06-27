using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;

namespace InventoryModule.Application.Commands;

public sealed record CreateIllustration : ICommand<EntityId<Guid>>
{
    public required string Name { get; init; }
    public string? ImageUrl { get; init; }
    public required float Width { get; init; }
    public required float Height { get; init; }
}

internal sealed class CreateIllustrationHandler(InventoryDbContext dbContext)
    : ICommandHandler<CreateIllustration, EntityId<Guid>>
{
    public async Task<Result<EntityId<Guid>, MessagingError>> Handle(CreateIllustration request, CancellationToken cancellationToken)
    {
        var illustration = new Illustration
        {
            Name = request.Name,
            NormalizedName = request.Name.ToUpperInvariant(),
            ImageUrl = request.ImageUrl,
            Width = request.Width,
            Height = request.Height,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var entityEntry = await dbContext.Set<Illustration>().AddAsync(illustration, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new EntityId<Guid>(entityEntry.Entity.Id);
    }
}