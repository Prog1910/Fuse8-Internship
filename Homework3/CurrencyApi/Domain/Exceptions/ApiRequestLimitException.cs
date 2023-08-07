using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Domain.Exceptions;

public class ApiRequestLimitException : HttpRequestException
{
    public ApiRequestLimitException() : base("You have reached your request limit") { }

    public ApiRequestLimitException(string? message) : base(message) { }

    public ApiRequestLimitException(string? message, Exception? inner) : base(message, inner) { }

    public ApiRequestLimitException(string? message, Exception? inner, HttpStatusCode? statusCode) : base(message, inner, statusCode) { }
}
