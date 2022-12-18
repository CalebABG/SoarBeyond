using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.EntityConfigurations;

public class MomentEntityConfiguration : IEntityTypeConfiguration<MomentEntity>
{
    public void Configure(EntityTypeBuilder<MomentEntity> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(MomentConstraints.TitleLength);

        builder.Property(m => m.Color)
            .HasMaxLength(MomentConstraints.ColorHexLength);

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(MomentConstraints.DescriptionLength);

        builder.Property(m => m.CreatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.UpdatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAddOrUpdate();

        builder.HasMany(m => m.Notes)
            .WithOne(n => n.Moment)
            .HasForeignKey(n => n.MomentId);

        builder.HasMany(m => m.Reflections)
            .WithOne(r => r.Moment)
            .HasForeignKey(r => r.MomentId);
    }
}