namespace SoarBeyond.Domain.Dto;

public class Moment
{
    public int Id { get; set; }
    public int JournalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Color { get; set; } = "#428bff";

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public virtual List<Note> Notes { get; set; } = new();
    public virtual List<Reflection> Reflections { get; set; } = new();
}