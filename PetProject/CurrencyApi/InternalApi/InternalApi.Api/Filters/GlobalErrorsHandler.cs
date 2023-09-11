using Domain.Errors;
using Grpc.Core;
using InternalApi.Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalApi.Api.Filters;

public sealed class GlobalErrorsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public GlobalErrorsHandler(ILogger<RequestLoggingMiddleware> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		Exception? error = context.Exception;

		if (error is RpcException rpcException)
		{
			Status status = rpcException.Status;
			error = status.DebugException;
			context.Result = new ObjectResult(new ProblemDetails
			{
				Title = error?.GetType().Name,
				Detail = status.Detail,
				Status = (int)status.StatusCode
			});
		}
		else
		{
			context.Result = new ObjectResult(new ProblemDetails
			{
				Title = error.GetType().Name,
				Detail = error.Message,
				Status = (int?)(error as HttpRequestException)?.StatusCode
			});
		}

		if (error is not CurrencyNotFoundException) LogError(error);

		context.ExceptionHandled = true;
	}

	private void LogError(Exception? exception)
	{
		_logger.LogError(exception, "An error occurred.");
	}
}
