using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fuse8_ByteMinds.SummerSchool.Application.Middleware;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggerMiddleware> _logger;

    public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        _logger.LogInformation("Request - {Method} {Path} {QueryString}", request.Method, request.Path, request.QueryString);
        await _next(context);
    }
}
