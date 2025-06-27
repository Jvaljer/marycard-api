using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoModule.Domain.Entities;

namespace VideoModule.Infrastructure.Configurations;

internal sealed class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasOne(card => card.Video)
            .WithOne()
            .HasForeignKey<Card>(card => card.VideoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(card => card.PreviewVideo)
            .WithMany()
            .HasForeignKey(card => card.PreviewVideoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(card => card.Group)
            .WithMany()
            .HasForeignKey(card => card.GroupId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}