namespace SoarBeyond.Shared.Dto;

public class Journal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public List<JournalEntry> JournalEntries { get; set; } = new();

    public int UserId { get; set; }
}