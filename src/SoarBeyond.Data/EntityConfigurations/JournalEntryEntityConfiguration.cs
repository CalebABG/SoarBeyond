using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.EntityConfigurations;

public class JournalEntryEntityConfiguration : IEntityTypeConfiguration<JournalEntryEntity>
{
    private const int TitleLength = 150;
    private const int DescriptionLength = 500;

    public void Configure(EntityTypeBuilder<JournalEntryEntity> builder)
    {
        builder.Property(je => je.Id)
            .UseIdentityColumn();

        builder.Property(je => je.Title)
            .HasMaxLength(TitleLength)
            .IsRequired();

        builder.Property(je => je.Content)
            .HasMaxLength(DescriptionLength)
            .IsRequired();

        builder.Property(je => je.CreationDate)
            .HasDefaultValue(DateTime.UtcNow)
            .ValueGeneratedOnAdd();

        builder.HasMany(journalEntry => journalEntry.Thoughts)
            .WithOne(thought => thought.JournalEntry)
            .HasForeignKey(thought => thought.JournalEntryId);
    }
}