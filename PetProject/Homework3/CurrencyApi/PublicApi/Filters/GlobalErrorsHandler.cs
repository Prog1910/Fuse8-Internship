using System.Net;
using Application.Common.Errors;
using Grpc.Core;
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
		var type = error.GetType().Name;

		if (error is RpcException rpcException)
		{
			type = rpcException.Trailers.GetValue("ExceptionType");
			context.Result = new ObjectResult(new ProblemDetails
			{
				Title = type,
				Status = (int?)rpcException.StatusCode,
			});

			if (type?.Contains(nameof(ApiRequestLimitException)) ?? false)
				LogError(rpcException);
		}
		else if (type?.Contains(nameof(CurrencyNotFoundException)) == false)
			LogError(error);

		context.ExceptionHandled = true;
	}

	private void LogError(Exception? exception)
		=> _logger.LogError(exception, $"An error occurred.");
}
