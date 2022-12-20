using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.EntityConfigurations;

public class NoteEntityConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Details)
            .IsRequired()
            .HasMaxLength(NoteConstraints.DetailsLength);
    }
}