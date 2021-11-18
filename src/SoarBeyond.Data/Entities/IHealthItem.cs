namespace SoarBeyond.Data.Entities;

public interface IGenericHealthItem<TKey>
{
    TKey UserId { get; set; }
}

public interface IHealthItem : IGenericHealthItem<int> { }