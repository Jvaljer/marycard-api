using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AryDotNet.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Domain.Entities;

[Index(nameof(TagId), IsUnique = true)]
internal sealed class PhysicalCard
{
    /// <summary>
    /// The video card id is not used as the primary key because some video card are only virtual. This means
    /// we do not have a physical card to represent it.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public required string VideoCardId { get; set; }

    public required Guid TagId { get; set; }

    [ForeignKey(nameof(Illustration))]
    public Guid? IllustrationId { get; set; }

    [MaxLength(FieldSize.VerySmallStringLength)]
    public string? CountryCodeWarehouse { get; set; }

    [MaxLength(FieldSize.VeryLargeStringLength)]
    public string? Note { get; set; }

    public DateTimeOffset? SoldAt { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public Illustration? Illustration { get; set; }
}