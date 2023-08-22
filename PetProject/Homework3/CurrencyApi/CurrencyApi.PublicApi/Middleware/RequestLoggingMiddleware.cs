namespace CurrencyApi.PublicApi.Middleware;

public sealed class RequestLoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
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