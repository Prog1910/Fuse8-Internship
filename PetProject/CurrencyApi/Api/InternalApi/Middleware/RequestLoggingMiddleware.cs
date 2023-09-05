namespace InternalApi.Middleware;

public sealed class RequestLoggingMiddleware
{
	private readonly ILogger<RequestLoggingMiddleware> _logger;
	private readonly RequestDelegate _next;

	public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var request = context.Request;
		_logger.LogInformation(message: "Request - {Method} {Path} {QueryString}", request.Method, request.Path, request.QueryString);
		await _next(context);
	}
}