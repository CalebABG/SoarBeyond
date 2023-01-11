using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.Entities;

public class JournalEntity
{
    public int Id { get; set; }
    public bool Favored { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; }

    public int? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }

    public IEnumerable<MomentEntity> Moments { get; set; }

    public class Configuration : IEntityTypeConfiguration<JournalEntity>
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

            builder.HasMany(j => j.Moments)
                .WithOne(m => m.Journal)
                .HasForeignKey(m => m.JournalId);

            builder.HasOne(j => j.Category)
                .WithMany(c => c.Journals)
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}