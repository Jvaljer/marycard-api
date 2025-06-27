using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AryDotNet.Entity;
using Common.Shopify;

namespace InventoryModule.Domain.Entities;

internal sealed class Product
{
    // Only for internal use with navigation keys
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InternalId { get; set; }

    public required ShopifyProductId ShopifyProductId { get; set; }

    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string Name { get; set; }

    [Required, MaxLength(FieldSize.VeryLargeStringLength)]
    public required string Description { get; set; }

    [MaxLength(FieldSize.SmallStringLength)]
    public required string? SKU { get; set; }

    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string VariantName { get; set; }

    public required bool Deleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}