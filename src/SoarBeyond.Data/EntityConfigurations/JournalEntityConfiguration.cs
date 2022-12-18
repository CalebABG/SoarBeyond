using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.EntityConfigurations;

public class JournalEntityConfiguration : IEntityTypeConfiguration<JournalEntity>
{
    public void Configure(EntityTypeBuilder<JournalEntity> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Name)
            .IsRequired()
            .HasMaxLength(JournalConstraints.NameLength);

        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(JournalConstraints.DescriptionLength);

        builder.Property(j => j.CreatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAdd();

        builder.Property(j => j.UpdatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAddOrUpdate();

        builder.HasMany(j => j.Moments)
            .WithOne(m => m.Journal)
            .HasForeignKey(m => m.JournalId);
    }
}