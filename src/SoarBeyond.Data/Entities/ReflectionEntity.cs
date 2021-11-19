namespace SoarBeyond.Data.Entities;

public class ReflectionEntity : IHealthItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public DateTime CreationDate { get; set; }

    // Relationships
    // public JournalEntry JournalEntry { get; set; }

    public int UserId { set; get; }
    public SoarBeyondUserEntity User { get; set; }
}