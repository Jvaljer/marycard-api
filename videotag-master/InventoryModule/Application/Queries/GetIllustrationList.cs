using AryDotNet.Messaging;
using AryDotNet.Result;
using Common.Paging;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Application.Queries;

public sealed record GetIllustrationList : IQuery<IReadOnlyList<IllustrationModel>>
{
    public required PageQuery Page { get; init; }
    public string? Name { get; init; }
}

internal sealed class GetIllustrationListHandler(InventoryDbContext dbContext)
    : IQueryHandler<GetIllustrationList, IReadOnlyList<IllustrationModel>>
{
    public async Task<Result<IReadOnlyList<IllustrationModel>, MessagingError>> Handle(GetIllustrationList request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Illustration>().AsNoTracking()
            .PagedOrderedDescending(request.Page, illustration => illustration.CreatedAt);

        if (request.Name is not null)
        {
            var normalizedName = request.Name.ToUpperInvariant();
            query = query.Where(illustration => illustration.NormalizedName.Contains(normalizedName));
        }

        var illustrations = await query.ToListAsync(cancellationToken);
        return illustrations.Select(Mapper.MapIllustration).ToList();
    }
}