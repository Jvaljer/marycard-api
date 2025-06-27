using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Domain.Entities;

[PrimaryKey(nameof(IllustrationId), nameof(ProductId))]
internal sealed class IllustrationProductAssociation
{
    [ForeignKey(nameof(Illustration))]
    public required Guid IllustrationId { get; set; }

    [ForeignKey(nameof(Product))]

    public required Guid ProductId { get; set; }

    public Illustration? Illustration { get; set; }
    public Product? Product { get; set; }
}