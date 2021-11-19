using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class ThoughtEntityConfiguration : IEntityTypeConfiguration<ThoughtEntity>
{
    private const int ColorLength = 16;
    private const int DetailsLength = 500;

    public void Configure(EntityTypeBuilder<ThoughtEntity> builder)
    {
        builder.Property(o => o.Id)
            .UseIdentityColumn();

        builder.Property(o => o.Details)
            .HasMaxLength(DetailsLength)
            .IsRequired();

        builder.Property(o => o.Color)
            .HasMaxLength(ColorLength);

        builder.Property(o => o.CreationDate)
            .HasDefaultValue(DateTime.UtcNow)
            .ValueGeneratedOnAdd();
    }
}