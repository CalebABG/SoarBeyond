using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class JournalEntityConfiguration : IEntityTypeConfiguration<JournalEntity>
{
    private const int NameLength = 150;
    private const int DescriptionLength = 500;

    public void Configure(EntityTypeBuilder<JournalEntity> builder)
    {
        builder.Property(j => j.Id)
            .UseIdentityColumn();

        builder.Property(j => j.Name)
            .HasMaxLength(NameLength)
            .IsRequired();

        builder.Property(j => j.Description)
            .HasMaxLength(DescriptionLength)
            .IsRequired();

        builder.Property(j => j.CreationDate)
            .HasDefaultValue(DateTime.UtcNow)
            .ValueGeneratedOnAdd();

        builder.HasMany(journal => journal.JournalEntries)
            .WithOne(journalEntry => journalEntry.Journal)
            .HasForeignKey(journalEntry => journalEntry.JournalId);
    }
}