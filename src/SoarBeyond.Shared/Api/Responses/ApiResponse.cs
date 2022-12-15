namespace SoarBeyond.Shared.Api.Responses;

public class ApiResponse<T> : ApiResponse
{
    public T Payload { get; set; }
}

public class ApiResponse
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}