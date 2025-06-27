using System.ComponentModel.DataAnnotations;
using AryDotNet.Entity;

namespace JobModule.Domain;

internal sealed class Job : Entity<Guid>
{
    [Required, MaxLength(FieldSize.LargeStringLength)]
    public required string Name { get; set; }
    public required JobStatus Status { get; set; }

    [MaxLength(FieldSize.VeryLargeStringLength)]
    public string? FailureReason { get; set; }
    public int? FailureCode { get; set; }
}