using Application.Common.Errors;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;

public class GlobalErrorsHandler : IExceptionFilter
{
	private readonly ILogger<RequestsLoggerMiddleware> _logger;

	public GlobalErrorsHandler(ILogger<RequestsLoggerMiddleware> logger) => _logger = logger;

	public void OnException(ExceptionContext context)
	{
		var error = context.Exception as HttpRequestException;

		context.Result = new ObjectResult(new ProblemDetails
		{
			Title = error?.Message,
			Status = (int?)error?.StatusCode
		});

		switch (error)
		{
			case ApiRequestLimitException:
				LogError(error);
				break;

			default:
				LogError(error);
				break;
		};

		context.ExceptionHandled = true;
	}

	private void LogError(Exception? exception)
	{
		_logger.LogError(exception, "An error occurred.");
	}
}