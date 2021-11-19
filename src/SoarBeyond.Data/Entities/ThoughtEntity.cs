namespace SoarBeyond.Data.Entities;

public class ThoughtEntity : IHealthItem
{
    public int Id { get; set; }
    public string Details { get; set; }
    public string Color { get; set; }
    public DateTime CreationDate { get; set; }

    // Relationships
    public int JournalEntryId { get; set; }
    public virtual JournalEntryEntity JournalEntry { get; set; }

    public int UserId { get; set; }
    public virtual SoarBeyondUserEntity User { get; set; }
}