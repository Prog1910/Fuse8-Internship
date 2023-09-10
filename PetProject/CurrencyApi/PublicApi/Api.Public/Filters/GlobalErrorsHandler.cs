using Api.Public.Middleware;
using Domain.Errors;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Public.Filters;

public sealed class GlobalErrorsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public GlobalErrorsHandler(ILogger<RequestLoggingMiddleware> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		Exception error = context.Exception;

		if (error is RpcException rpcException)
		{
			Status status = rpcException.Status;
			Exception? exception = status.DebugException;
			context.Result = new ObjectResult(new ProblemDetails
			{
				Title = exception?.GetType().Name,
				Detail = status.Detail,
				Status = (int)status.StatusCode
			});

			if (exception is not CurrencyNotFoundException) LogError(exception);
		}
		else
		{
			if (error is not CurrencyNotFoundException)
			{
				context.Result = new ObjectResult(new ProblemDetails
				{
					Title = error.GetType().Name,
					Detail = error.Message
				});
				LogError(error);
			}
		}

		context.ExceptionHandled = true;
	}

	private void LogError(Exception? exception)
	{
		_logger.LogError(exception, "An error occurred.");
	}
}
