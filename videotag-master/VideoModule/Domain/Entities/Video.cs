using System.ComponentModel.DataAnnotations;
using AryDotNet.Entity;

namespace VideoModule.Domain.Entities;

internal sealed class Video : Entity<Guid>
{
    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string Title { get; set; }
    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string NormalizedTitle { get; set; }

    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string ApiVideoId { get; set; }

    [Required]
    public required bool Playable { get; set; }

    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string VideoUrl { get; set; }
}