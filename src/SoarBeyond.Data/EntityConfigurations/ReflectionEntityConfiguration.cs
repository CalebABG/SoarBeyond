using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class ReflectionEntityConfiguration : IEntityTypeConfiguration<ReflectionEntity>
{
    private const int TitleLength = 150;
    private const int DetailsLength = 500;

    public void Configure(EntityTypeBuilder<ReflectionEntity> builder)
    {
        builder.Property(r => r.Id)
            .UseIdentityColumn();

        builder.Property(r => r.Title)
            .HasMaxLength(TitleLength)
            .IsRequired();

        builder.Property(r => r.Details)
            .HasMaxLength(DetailsLength)
            .IsRequired();

        builder.Property(r => r.CreationDate)
            .HasDefaultValue(DateTime.UtcNow)
            .ValueGeneratedOnAdd();
    }
}