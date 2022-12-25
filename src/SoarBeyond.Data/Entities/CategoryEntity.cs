namespace SoarBeyond.Data.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public IEnumerable<JournalEntity> Journals { get; set; }
}