using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderModule.Domain;

namespace OrderModule.Infrastructure.Configuration;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(orderItem => orderItem.Product)
            .WithMany(orderProduct => orderProduct.Items)
            .HasForeignKey(orderItem => orderItem.OrderProductId)
            .IsRequired(true);
    }
}