namespace SoarBeyond.Data.Entities;

public class ReflectionEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int MomentId { get; set; }
    public MomentEntity Moment { get; set; }
}