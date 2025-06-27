using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AryDotNet.Entity;

namespace VideoModule.Domain.Entities;

internal sealed class Group
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public required string Name { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public required string NormalizedName { get; set; }

    [ForeignKey(nameof(PreviewVideo))]
    public Guid? PreviewVideoId { get; set; }

    public PreviewVideo? PreviewVideo { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}