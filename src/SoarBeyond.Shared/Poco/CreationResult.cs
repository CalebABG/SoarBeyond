namespace SoarBeyond.Shared.Poco;

public class CreationResult<T>
{
    public bool Succeeded { get; set; }
    public string Error { get; set; } = string.Empty;

    public T Item { get; set; }

    public static CreationResult<T> Success(T result) => new()
    {
        Succeeded = true,
        Item = result
    };

    public static CreationResult<T> Fail(T result = default, string reason = "") => new()
    {
        Succeeded = false,
        Error = reason,
        Item = result
    };
}