namespace SoarBeyond.Shared.Dto;

public class Reflection
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
}