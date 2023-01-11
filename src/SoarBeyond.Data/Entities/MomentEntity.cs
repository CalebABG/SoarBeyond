using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.Entities;

public class MomentEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Color { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int JournalId { get; set; }
    public JournalEntity Journal { get; set; }

    public IEnumerable<NoteEntity> Notes { get; set; }
    public IEnumerable<ReflectionEntity> Reflections { get; set; }

    public class Configuration : IEntityTypeConfiguration<MomentEntity>
    {
        public void Configure(EntityTypeBuilder<MomentEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(MomentConstraints.TitleLength);

            builder.Property(m => m.Color)
                .HasMaxLength(MomentConstraints.ColorHexLength);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(MomentConstraints.DescriptionLength);

            builder.HasMany(m => m.Notes)
                .WithOne(n => n.Moment)
                .HasForeignKey(n => n.MomentId);

            builder.HasMany(m => m.Reflections)
                .WithOne(r => r.Moment)
                .HasForeignKey(r => r.MomentId);
        }
    }
}