using AnalyticsModule.Domain;
using AnalyticsModule.Models;

namespace AnalyticsModule.Application;

internal static class Mapper
{
    public static ActivityEventModel MapEventModel(ActivityEvent evt) => new()
    {
        Id = evt.Id,
        Type = evt.Type,
        CardId = evt.CardId,
        CreatedAt = evt.CreatedAt,
        UpdatedAt = evt.UpdatedAt
    };
}