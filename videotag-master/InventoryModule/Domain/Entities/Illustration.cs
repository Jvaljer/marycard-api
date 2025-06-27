using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AryDotNet.Entity;

namespace InventoryModule.Domain.Entities;

internal sealed class Illustration
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public required string Name { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public required string NormalizedName { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? ImageUrl { get; set; }

    public required float Width { get; set; }

    public required float Height { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
}