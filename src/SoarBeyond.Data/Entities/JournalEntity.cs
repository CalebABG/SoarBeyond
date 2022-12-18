namespace SoarBeyond.Data.Entities;

public class JournalEntity
{
    public int Id { get; set; }
    public bool Favored { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }

    public virtual IEnumerable<MomentEntity> Moments { get; set; }
}