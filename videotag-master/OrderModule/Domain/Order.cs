using System.ComponentModel.DataAnnotations;
using AryDotNet.Entity;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;

namespace OrderModule.Domain;

[Index(nameof(ShopifyOrderId))]
[Index(nameof(ShopifyCustomerId))]
internal sealed class Order : Entity<Guid>
{
    public required ulong ShopifyOrderId { get; set; }
    public required ulong ShopifyCustomerId { get; set; }
    public required OrderState State { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerFirstName { get; set; }
    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerFirstNameNormalized { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerLastName { get; set; }
    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerLastNameNormalized { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerEmail { get; set; }
    [MaxLength(FieldSize.LargeStringLength)]
    public string? CustomerEmailNormalized { get; set; }

    [MaxLength(FieldSize.LargeStringLength)]
    public string? ContactEmail { get; set; }
    [MaxLength(FieldSize.LargeStringLength)]
    public string? ContactEmailNormalized { get; set; }

    [MaxLength(FieldSize.MediumLargeStringLength)]
    public string? CustomerPhone { get; set; }

    [MaxLength(FieldSize.VeryLargeStringLength)]
    public string? Note { get; set; }
    [MaxLength(FieldSize.VeryLargeStringLength)]
    public string? NoteNormalized { get; set; }

    public DateTime ShopifyCreatedAt { get; set; }
    public DateTime ShopifyUpdatedAt { get; set; }

    public ICollection<OrderProduct> Products { get; set; } = new List<OrderProduct>();
}