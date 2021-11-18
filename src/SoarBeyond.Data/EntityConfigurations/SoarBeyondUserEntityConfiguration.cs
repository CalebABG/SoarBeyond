using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class SoarBeyondUserEntityConfiguration : IEntityTypeConfiguration<SoarBeyondUserEntity>
{
    public void Configure(EntityTypeBuilder<SoarBeyondUserEntity> builder)
    {
        builder.Property(p => p.CreationDate)
            .HasDefaultValue(DateTime.UtcNow)
            .ValueGeneratedOnAdd();
    }
}