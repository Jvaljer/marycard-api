using System.ComponentModel.DataAnnotations;
using AryDotNet.Entity;

namespace VideoModule.Domain.Entities;

internal sealed class Card
{
    [Key, MaxLength(FieldSize.LargeStringLength)]
    public required string Identifier { get; set; }

    [Required]
    public required bool Locked { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? Url { get; set; }
    public Guid? VideoId { get; set; }
    public Guid? PreviewVideoId { get; set; }
    public Guid? GroupId { get; set; }
    public Video? Video { get; set; }
    public PreviewVideo? PreviewVideo { get; set; }
    public Group? Group { get; set; }


    [Obsolete, MaxLength(FieldSize.LargeStringLength)]
    public string? VideoTitle { get; set; }

    [Obsolete("Please do not use"), MaxLength(FieldSize.LargeStringLength)]
    public string? ApiVideoId { get; set; }

    [Obsolete("Please do not use")]
    public bool? Playable { get; set; }

    [Obsolete("Please do not use"), MaxLength(FieldSize.LargeStringLength)]
    public string? VideoUrl { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
}