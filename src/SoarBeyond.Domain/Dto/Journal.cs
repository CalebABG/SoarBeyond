namespace SoarBeyond.Domain.Dto;

public class Journal
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Favored { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
    public List<Moment> Moments { get; set; } = new();
}