using Fuse8_ByteMinds.SummerSchool.Application.Middleware;
using Fuse8_ByteMinds.SummerSchool.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Application.Filters;

public class GlobalExceptionsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggerMiddleware> _logger;

	public GlobalExceptionsHandler(ILogger<RequestLoggerMiddleware> logger) => _logger = logger;

	public void OnException(ExceptionContext context)
	{
		switch (context.Exception)
		{
			case ApiRequestLimitException:
				context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
				break;

			case CurrencyNotFoundException:
				context.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
				_logger.LogError(context.Exception, context.Exception.Message);
				break;

			default:
				context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
				_logger.LogError(context.Exception, context.Exception.Message);
				break;
		};
	}
}