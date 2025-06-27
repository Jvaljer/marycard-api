using System.Net;
using AryDotNet.Messaging;
using InventoryApi.Models;
using InventoryModule.Domain.Entities;
using InventoryModule.Models;
using Microsoft.AspNetCore.Http;
using TagClient.Models;
using TagClient.Models.Tag;

namespace InventoryModule.Application;

internal static class Mapper
{
    public static ProductModel ProductToProductModel(Product product)
    {
        return new ProductModel
        {
            Name = product.Name,
            VariantName = product.VariantName,
            Description = product.Description,
            SKU = product.SKU,
            ShopifyProductId = product.ShopifyProductId,
            Deleted = product.Deleted,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public static PhysicalCardModel MapPhysicalCard(PhysicalCard p, TagModel? t = null) => new()
    {
        Id = p.Id,
        VideoCardId = p.VideoCardId,
        Illustration = p.Illustration is null ? null : MapIllustration(p.Illustration),
        Note = p.Note,
        WarehouseCountryCode = p.CountryCodeWarehouse,
        PhysicalTag = t is null ? null : MapPhysicalTagModel(t),
        TagId = p.TagId,
        SoldAt = p.SoldAt,
        CreatedAt = p.CreatedAt
    };

    public static IllustrationModel MapIllustration(Illustration illustration) => new IllustrationModel
    {
        Id = illustration.Id,
        Name = illustration.Name,
        ImageUrl = illustration.ImageUrl,
        Width = illustration.Width,
        Height = illustration.Height,
        CreatedAt = illustration.CreatedAt
    };

    public static MessagingError MapTagError(Error err) => err.Code switch
    {
        StatusCodes.Status404NotFound => new MessagingError(HttpStatusCode.NotFound, err.Message ?? "Not found"),
        StatusCodes.Status400BadRequest => new MessagingError(HttpStatusCode.BadRequest, err.Message ?? "Bad request"),
        StatusCodes.Status403Forbidden => new MessagingError(HttpStatusCode.Forbidden, err.Message ?? "Forbidden"),
        StatusCodes.Status401Unauthorized => new MessagingError(HttpStatusCode.Unauthorized,
            err.Message ?? "Unauthorized"),
        _ => new MessagingError(HttpStatusCode.NotFound, err.Message ?? "Server error")
    };

    public static PhysicalTagModel MapPhysicalTagModel(TagModel t) => new()
    {
        Id = t.Id,
        PhysicalUid = t.Uid,
        UsageCount = t.UsageCounter,
        LastTagCounter = t.LastTagCounter,
        SignatureInvalidCount = t.SignatureInvalidCounter,
        SignatureValidCount = t.SignatureValidCounter
    };
}