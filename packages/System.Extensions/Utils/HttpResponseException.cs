using System.Net;

namespace Pomodorium.Utils;

public class HttpResponseException : Exception
{
    public int Status { get; }

    private readonly string? _message;

    public object? Value { get; }

    public override string Message =>
        _message ?? $"{Status}: {Value}";

    public HttpStatusCode StatusCode => (HttpStatusCode)Status;

    public HttpResponseException(HttpStatusCode statusCode = HttpStatusCode.InternalServerError, object? value = null, string? message = null)
    {
        Value = value;
        Status = (int)statusCode;
        _message = message;
    }
}
