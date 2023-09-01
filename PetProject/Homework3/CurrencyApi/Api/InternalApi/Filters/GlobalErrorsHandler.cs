using Domain.Errors;
using InternalApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalApi.Filters;

public sealed class GlobalErrorsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public GlobalErrorsHandler(ILogger<RequestLoggingMiddleware> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		var error = context.Exception;

		context.Result = new ObjectResult(new ProblemDetails
		{
			Title = error?.Message,
			Status = (int?)(error as HttpRequestException)?.StatusCode
		});

		switch (error)
		{
			case ApiRequestLimitException:
				LogError(error);
				break;

			default:
				LogError(error);
				break;
		}
		;

		context.ExceptionHandled = true;
	}

	private void LogError(Exception? exception)
	{
		_logger.LogError(exception, message: "An error occurred.");
	}
}