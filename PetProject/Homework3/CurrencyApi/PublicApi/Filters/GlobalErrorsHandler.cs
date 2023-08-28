using Application.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PublicApi.Middleware;

namespace PublicApi.Filters;

public sealed class GlobalErrorsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public GlobalErrorsHandler(ILogger<RequestLoggingMiddleware> logger) => _logger = logger;

	public void OnException(ExceptionContext context)
	{
		var error = context.Exception;

		while (error.InnerException != null)
		{
			error = error.InnerException;
			if (error is CurrencyNotFoundException currencyNotFoundException)
			{
				context.Result = new ObjectResult(new ProblemDetails
				{
					Title = currencyNotFoundException.Message,
					Status = (int?)currencyNotFoundException.StatusCode,
				});

				LogError(currencyNotFoundException);
				context.ExceptionHandled = true;
				return;
			}
		}

		context.Result = new ObjectResult(new ProblemDetails
		{
			Title = error?.Message,
			Status = (int?)(error as HttpRequestException)?.StatusCode,
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
		=> _logger.LogError(exception, $"An error occurred.");
}