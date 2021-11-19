namespace SoarBeyond.Data.Entities;

public class JournalEntity : IHealthItem
{
    public JournalEntity()
    {
        JournalEntries = new List<JournalEntryEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }

    // Relationships
    public int UserId { get; set; }
    public virtual SoarBeyondUserEntity User { get; set; }

    public virtual List<JournalEntryEntity> JournalEntries { get; set; }
}