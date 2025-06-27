using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetIllustrationById : IQuery<IllustrationModel>
{
    public required Guid IllustrationId { get; init; }
}

internal sealed class GetIllustrationByIdHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetIllustrationById, IllustrationModel>
{
    public async Task<Result<IllustrationModel, MessagingError>> Handle(GetIllustrationById request,
        CancellationToken cancellationToken)
    {
        var illustration = await dbContext.Set<Illustration>()
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == request.IllustrationId, cancellationToken);

        if (illustration is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Illustration not found.");
        }

        return Mapper.MapIllustration(illustration);
    }
}