namespace SoarBeyond.Data.Entities;

public class JournalEntryEntity : IHealthItem
{
    public JournalEntryEntity()
    {
        Thoughts = new List<ThoughtEntity>();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }


    // Relationships
    public int JournalId { get; set; }
    public virtual JournalEntity Journal { get; set; }

    public int UserId { get; set; }
    public virtual SoarBeyondUserEntity User { get; set; }

    public virtual List<ThoughtEntity> Thoughts { get; set; }
}