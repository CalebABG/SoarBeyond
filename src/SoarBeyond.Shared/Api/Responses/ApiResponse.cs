namespace SoarBeyond.Shared.Api.Responses;

public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public T Payload { get; set; }

    public static ApiResponse<T> Success(T result) => new() { Succeeded = true, Payload = result };
    public static ApiResponse<T> Fail(T result = default) => new() { Succeeded = false, Payload = result };
}