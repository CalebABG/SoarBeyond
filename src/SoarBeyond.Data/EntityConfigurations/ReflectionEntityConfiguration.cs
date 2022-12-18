using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.EntityConfigurations;

public class ReflectionEntityConfiguration : IEntityTypeConfiguration<ReflectionEntity>
{
    public void Configure(EntityTypeBuilder<ReflectionEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(ReflectionConstraints.TitleLength);

        builder.Property(r => r.Details)
            .IsRequired()
            .HasMaxLength(ReflectionConstraints.DetailsLength);

        builder.Property(r => r.CreatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.UpdatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAddOrUpdate();
    }
}