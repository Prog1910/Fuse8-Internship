namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Middleware;

public class RequestsLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestsLoggerMiddleware> _logger;

    public RequestsLoggerMiddleware(RequestDelegate next, ILogger<RequestsLoggerMiddleware> logger)
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