using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.Entities;

public class NoteEntity
{
    public int Id { get; set; }
    public string Details { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int MomentId { get; set; }
    public MomentEntity Moment { get; set; }

    public class Configuration : IEntityTypeConfiguration<NoteEntity>
    {
        public void Configure(EntityTypeBuilder<NoteEntity> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Details)
                .IsRequired()
                .HasMaxLength(NoteConstraints.DetailsLength);
        }
    }
}