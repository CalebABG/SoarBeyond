using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(u => u.CreatedDate)
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .ValueGeneratedOnAdd();
    }
}