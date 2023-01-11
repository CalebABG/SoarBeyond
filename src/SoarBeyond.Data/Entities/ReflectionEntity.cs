using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.Entities;

public class ReflectionEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int MomentId { get; set; }
    public MomentEntity Moment { get; set; }

    public class Configuration : IEntityTypeConfiguration<ReflectionEntity>
    {
        public void Configure(EntityTypeBuilder<ReflectionEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(ReflectionConstraints.TitleLength);

            builder.Property(r => r.Details)
                .IsRequired()
                .HasMaxLength(ReflectionConstraints.DetailsLength);
        }
    }
}