using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.EntityConfigurations;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(CategoryConstraints.NameLength);

        builder.Property(c => c.Description)
            .HasMaxLength(CategoryConstraints.DescriptionLength);

        builder.HasMany(c => c.Journals)
            .WithOne(j => j.Category)
            .HasForeignKey(j => j.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}