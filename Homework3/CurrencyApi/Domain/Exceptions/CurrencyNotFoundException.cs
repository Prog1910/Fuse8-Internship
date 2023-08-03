using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Domain.Exceptions;

public class CurrencyNotFoundException : HttpRequestException
{
    public CurrencyNotFoundException() : base("Unknown currency") { }

    public CurrencyNotFoundException(string? message) : base(message) { }

    public CurrencyNotFoundException(string? message, Exception? inner) : base(message, inner) { }

    public CurrencyNotFoundException(string? message, Exception? inner, HttpStatusCode? statusCode) : base(message, inner, statusCode) { }
}
