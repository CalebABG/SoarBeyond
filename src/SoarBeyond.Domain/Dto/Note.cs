namespace SoarBeyond.Domain.Dto;

public class Note
{
    public int Id { get; set; }
    public int MomentId { get; set; }
    public string Details { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
}