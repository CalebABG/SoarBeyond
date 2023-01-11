using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoarBeyond.Shared;

namespace SoarBeyond.Data.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public IEnumerable<JournalEntity> Journals { get; set; }

    public class Configuration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(CategoryConstraints.NameLength);

            builder.Property(c => c.Description)
                .HasMaxLength(CategoryConstraints.DescriptionLength);
        }
    }
}