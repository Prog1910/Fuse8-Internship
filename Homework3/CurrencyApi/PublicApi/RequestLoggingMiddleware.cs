using Microsoft.AspNetCore.Http;
using System.IO;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

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

		_logger.LogInformation("Incoming Request - {Method} {Path} {QueryString}",
			request.Method, request.Path, request.QueryString);
		await _next(context);
	}
}
