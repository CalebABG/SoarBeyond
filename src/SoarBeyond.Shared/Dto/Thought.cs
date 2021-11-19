namespace SoarBeyond.Shared.Dto;

public class Thought
{
    public int Id { get; set; }
    public string Details { get; set; } = string.Empty;
    public string Color { get; set; } = "#428bff";
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public int JournalEntryId { get; set; }

    public int UserId { get; set; }
}